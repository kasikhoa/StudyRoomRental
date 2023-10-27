using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Order;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Repository.Interfaces;
using System.Collections.Generic;

namespace StudyRoomRental.API.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<OrderService> logger) : base(unitOfWork, logger)
        {

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


            foreach(var room in request.RoomList)
            {
                bool isScheduleMatched = roomSchedules.Any(schedule =>
                    (room.StartTime >= schedule.StartTime && room.StartTime < schedule.EndTime) ||
                    (room.EndTime > schedule.StartTime && room.EndTime <= schedule.EndTime));

                if (!isScheduleMatched) throw new BadHttpRequestException(MessageConstant.RoomSchedule.ScheduleNotMatchedMessage); ; 
                  
                orderItems.Add(new OrderItem()
                {
                    OrderId = newOrder.Id,
                    RoomId = room.RoomId,
                    StartTime = room.StartTime,
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
    }
}
