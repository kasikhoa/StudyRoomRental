using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Feedback;
using StudyRoomRental.BusinessTier.Payload.Order;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest request);
        Task<IPaginate<GetOrderDetailResponse>> ViewAllOrders(int? accountId, OrderStatus? status, int page, int size);
        Task<GetOrderDetailResponse> GetOrderDetail(Guid id);
        Task<UpdateOrderResponse> UpdateOrder(Guid id, UpdateOrderRequest request);
        Task<bool> CancelOrder(Guid id);
        Task<FeedBackResponse> UpdateOrderFeedback(Guid id, FeedBackRequest request);
        Task<IPaginate<FeedBackResponse>> ViewAllFeedbacks(Guid? orderId, int page, int size);
    }
}
