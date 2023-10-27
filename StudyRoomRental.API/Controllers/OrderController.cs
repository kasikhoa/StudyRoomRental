using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Feedback;
using StudyRoomRental.BusinessTier.Payload.Order;
using StudyRoomRental.BusinessTier.Payload.RoomSchedule;

namespace StudyRoomRental.API.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [HttpPost(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewOrder(CreateOrderRequest request)
        {
            var response = await _orderService.CreateNewOrder(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllOrders(int? accountId, OrderStatus? status, int page, int size)
        {
            var response = await _orderService.ViewAllOrders(accountId, status, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrderEndPoint)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllOrders(Guid id)
        {
            var response = await _orderService.GetOrderDetail(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.Order.OrderEndPoint)]
        [ProducesResponseType(typeof(UpdateOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllOrders(Guid id, UpdateOrderRequest request)
        {
            var response = await _orderService.UpdateOrder(id, request);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Order.OrderEndPoint)]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var isSuccessful = await _orderService.CancelOrder(id);
            if (!isSuccessful) return Ok(MessageConstant.Order.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Order.UpdateStatusSuccessMessage);
        }

        [HttpPost(ApiEndPointConstant.Order.FeedbackEndPoint)]
        [ProducesResponseType(typeof(FeedBackResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderFeedback(Guid id, FeedBackRequest request)
        {
            var response = await _orderService.UpdateOrderFeedback(id, request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.FeedbacksEndPoint)]
        [ProducesResponseType(typeof(FeedBackResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllFeedbacks(Guid? id, int page, int size)
        {
            var response = await _orderService.ViewAllFeedbacks(id, page, size);
            return Ok(response);
        }
    }
}
