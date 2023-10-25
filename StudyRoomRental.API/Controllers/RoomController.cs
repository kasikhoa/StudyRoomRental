using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Room;

namespace StudyRoomRental.API.Controllers
{
    [ApiController]
    public class RoomController : BaseController<RoomController>
    {
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService) : base(logger)
        {
                _roomService = roomService;
        }

        [HttpPost(ApiEndPointConstant.Room.RoomsEndPoint)]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateCategory(CreateRoomRequest request)
        {
            var response = await _roomService.CreateNewRoom(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Room.RoomsEndPoint)]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllRooms(int? accountId, int? roomTypeId, string? name, RoomStatus? status, int page, int size)
        {
            var response = await _roomService.ViewAllRooms(accountId, roomTypeId, name, status, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Room.RoomEndPoint)]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var response = await _roomService.GetRoomById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.Room.RoomEndPoint)]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateRoomInformation(int id, UpdateRoomRequest request)
        {
            var response = await _roomService.UpdateRoomInformation(id, request);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Room.RoomEndPoint)]
        [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateRoomStatus(int id)
        {
            var response = await _roomService.UpdateRoomStatus(id);
            return Ok(response);
        }

    }
}
