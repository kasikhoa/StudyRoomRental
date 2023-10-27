using StudyRoomRental.BusinessTier.Payload.Order;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest request);
    }
}
