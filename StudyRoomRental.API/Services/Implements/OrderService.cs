using Microsoft.EntityFrameworkCore;
using StudyRoomRental.API.Extensions;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Feedback;
using StudyRoomRental.BusinessTier.Payload.Order;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;
using StudyRoomRental.DataTier.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudyRoomRental.API.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<OrderService> logger) : base(unitOfWork, logger)
        {

        }

        private async Task<bool> IsRoomAvailableForBooking(int roomId, DateTime startTime, DateTime endTime)
        {
            ICollection<OrderItem> listOrderItem = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                predicate: x => x.RoomId.Equals(roomId));
            var isRoomBooked = listOrderItem.Any(item => item.RoomId.Equals(roomId) &&
                (startTime >= item.StartTime && startTime < item.EndTime) ||
                (endTime > item.StartTime && endTime <= item.EndTime));

            ICollection<RoomSchedule> listRoomSchedules = await _unitOfWork.GetRepository<RoomSchedule>().GetListAsync(
                predicate: x => x.RoomId.Equals(roomId));
            var isRoomScheduleAvailable = listRoomSchedules.Any(item => item.RoomId.Equals(roomId) &&
                (startTime >= item.StartTime && startTime < item.EndTime) ||
                (endTime > item.StartTime && endTime <= item.EndTime));

            return isRoomScheduleAvailable && !isRoomBooked;
        }

        private async Task<DateTime> CalculateCleaningTime(int roomId, DateTime startTime, DateTime endTime)
        {
            ICollection<OrderItem> roomBookings = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                predicate: x => x.RoomId.Equals(roomId),
                orderBy: x => x.OrderBy(x => x.StartTime)
            );

            foreach (var booking in roomBookings)
            {
                if (startTime.Equals(booking.EndTime))
                {
                    startTime = startTime.AddMinutes(30);
                }
            }
            return startTime;
        }

        public async Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.AccountId));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            Order newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                CreatedTime = TimeUtils.GetCurrentSEATime(),
                TotalAmount = request.TotalAmount,
                Status = OrderStatus.Pending.GetDescriptionFromEnum(),
            };

            List<OrderItem> orderItems = new List<OrderItem>();
            int count = 0;

            List<int> roomIds = request.RoomList.Select(room => room.RoomId).ToList();
            ICollection<RoomSchedule> roomSchedules = await _unitOfWork.GetRepository<RoomSchedule>().GetListAsync(
               predicate: x => roomIds.Contains(x.RoomId));


            foreach (var room in request.RoomList)
            {
                bool isRoomAvailable = await IsRoomAvailableForBooking(room.RoomId, room.StartTime, room.EndTime);

                if (!isRoomAvailable) throw new BadHttpRequestException(MessageConstant.RoomSchedule.ScheduleNotMatchedMessage);

                DateTime newStartTime = await CalculateCleaningTime(room.RoomId, room.StartTime, room.EndTime);
                orderItems.Add(new OrderItem()
                {
                    OrderId = newOrder.Id,
                    RoomId = room.RoomId,
                    StartTime = newStartTime,
                    EndTime = room.EndTime,
                    CostPrice = room.CostPrice,
                });
                count++;
            }

            newOrder.RoomQuantity = count;
            await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();

            return new CreateOrderResponse()
            {
                Id = newOrder.Id,
                Account = account.Email,
                CreatedTime = newOrder.CreatedTime,
                RoomQuantity = newOrder.RoomQuantity,

            };
        }

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(int? accountId, OrderStatus? status)
        {
            Expression<Func<Order, bool>> filterQuery = p => true;

            if (accountId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.AccountId.Equals(accountId));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }



            return filterQuery;
        }

        public async Task<IPaginate<GetOrderDetailResponse>> ViewAllOrders(int? accountId, OrderStatus? status, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            var orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrderDetailResponse()
                {
                    Id = x.Id,
                    Account = x.Account.Email,
                    CreatedTime = x.CreatedTime,
                    RoomQuantity = x.RoomQuantity,
                    TotalAmount = x.TotalAmount,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status)

                },
                predicate: BuildGetOrdersQuery(accountId, status),
                page: page,
                size: size
                );

            foreach (var order in orderList.Items)
            {
                order.RoomList = (List<OrderItemResponse>)await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                    selector: x => new OrderItemResponse()
                    {
                        Id = x.Id,
                        RoomName = x.Room.Name,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        CostPrice = x.CostPrice,
                    },
                    predicate: x => x.OrderId.Equals(order.Id)
                    );
            }
            return orderList;
        }

        public async Task<GetOrderDetailResponse> GetOrderDetail(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.EmptyIdMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Account)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            GetOrderDetailResponse result = new GetOrderDetailResponse();
            result.Id = order.Id;
            result.Account = order.Account.Email;
            result.CreatedTime = order.CreatedTime;
            result.RoomQuantity = order.RoomQuantity;
            result.TotalAmount = order.TotalAmount;
            result.Status = EnumUtil.ParseEnum<OrderStatus>(order.Status);
            result.RoomList = (List<OrderItemResponse>)await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                selector: x => new OrderItemResponse()
                {
                    Id = x.Id,
                    RoomName = x.Room.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    CostPrice = x.CostPrice,
                },
                predicate: x => x.OrderId.Equals(id)
                );
            return result;
        }

        public async Task<UpdateOrderResponse> UpdateOrder(Guid id, UpdateOrderRequest request)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.EmptyIdMessage);
            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
               predicate: x => x.Id.Equals(id)
               );
            if (updateOrder == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            OrderStatus status = request.Status;

            switch (status)
            {
                case OrderStatus.Completed:
                    updateOrder.Status = request.Status.GetDescriptionFromEnum();
                    updateOrder.CompletedTime = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);
                    return new UpdateOrderResponse()
                    {
                        CreatedTime = updateOrder.CreatedTime,
                        RoomQuantity = updateOrder.RoomQuantity,
                        TotalAmount = updateOrder.TotalAmount,
                        Status = EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                        CompletedTime = updateOrder.CompletedTime,
                        Message = MessageConstant.Order.CompletedStatusMessage
                    };
                default:
                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()) ||
                        updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(updateOrder.CreatedTime, updateOrder.RoomQuantity, updateOrder.TotalAmount, 
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedTime, MessageConstant.Order.CannotChangeToStatusMessage);

                    return new UpdateOrderResponse(updateOrder.CreatedTime, updateOrder.RoomQuantity, updateOrder.TotalAmount,
                           EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedTime, MessageConstant.Order.CompletedStatusMessage);
            }
        }

        public async Task<bool> CancelOrder(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.EmptyIdMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            order.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<FeedBackResponse> UpdateOrderFeedback(Guid orderId, FeedBackRequest request)
        {
            if (orderId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.EmptyIdMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            Feedback feedback = new Feedback()
            {
                OrderId = orderId,
                Rating = request.Rating,
                Content = request.Content,
            };
            await _unitOfWork.GetRepository<Feedback>().InsertAsync(feedback);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.FeedbackFailedMessage);
            return new FeedBackResponse(feedback.Id, feedback.Rating, feedback.Content);
        }

        public async Task<IPaginate<FeedBackResponse>> ViewAllFeedbacks(Guid? orderId, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10: size;

            IPaginate<FeedBackResponse> result = await _unitOfWork.GetRepository<Feedback>().GetPagingListAsync(
                selector: x => new FeedBackResponse(x.Id, x.Rating, x.Content),
                predicate: (!orderId.HasValue) ? x => true : x => x.OrderId.Equals(orderId),
                page: page,
                size: size               
                );
            return result;
        }
    }
}
