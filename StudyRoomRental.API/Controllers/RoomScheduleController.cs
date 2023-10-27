using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
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
        public async Task<IActionResult> CreateCategory(RoomScheduleRequest request)
        {
            var response = await _roomScheduleService.CreateRoomSchedule(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.RoomSchedule.RoomSchedulesEndPoint)]
        [ProducesResponseType(typeof(RoomScheduleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllRoomSchedules(int? roomId, DateTime? startTime, DateTime? endTime, RoomScheduleStatus? status,
            int page, int size)
        {
            var response = await _roomScheduleService.ViewAllRoomSchedules(roomId, startTime, endTime, status, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.RoomSchedule.RoomScheduleEndPoint)]
        [ProducesResponseType(typeof(RoomScheduleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoomScheduleById(int id)
        {
            var response = await _roomScheduleService.GetRoomScheduleById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.RoomSchedule.RoomScheduleEndPoint)]
        [ProducesResponseType(typeof(RoomScheduleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRoomSchedule(int id, UpdateRoomScheduleRequest request)
        {
            var response = await _roomScheduleService.UpdateRoomSchedule(id, request);
            return Ok(response);
        }
    }
}
