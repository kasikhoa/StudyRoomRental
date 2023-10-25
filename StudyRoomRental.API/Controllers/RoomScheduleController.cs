using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Payload.RoomSchedule;

namespace StudyRoomRental.API.Controllers
{
    [ApiController]
    public class RoomScheduleController : BaseController<RoomScheduleController>
    {
        private readonly IRoomScheduleService _roomScheduleService;

        public RoomScheduleController(ILogger<RoomScheduleController> logger, IRoomScheduleService roomScheduleService) : base(logger)
        {
            _roomScheduleService = roomScheduleService;
        }

        [HttpPost(ApiEndPointConstant.RoomSchedule.RoomSchedulesEndPoint)]
        [ProducesResponseType(typeof(RoomScheduleResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateCategory(RoomScheduleRequest request)
        {
            var response = await _roomScheduleService.CreateRoomSchedule(request);
            return Ok(response);
        }
    }
}
