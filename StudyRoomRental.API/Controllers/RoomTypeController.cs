using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Payload.RoomType;

namespace StudyRoomRental.API.Controllers
{
    [ApiController]
    public class RoomTypeController : BaseController<RoomTypeController>
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(ILogger<RoomTypeController> logger, IRoomTypeService roomTypeService) : base(logger)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpPost(ApiEndPointConstant.RoomType.RoomTypesEndPoint)]
        [ProducesResponseType(typeof(RoomTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCategory(RoomTypeRequest request)
        {
            var response = await _roomTypeService.CreateRoomType(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.RoomType.RoomTypesEndPoint)]
        [ProducesResponseType(typeof(RoomTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoomTypes(string? name, int page, int size)
        {
            var response = await _roomTypeService.GetRoomTypes(name, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.RoomType.RoomTypeEndPoint)]
        [ProducesResponseType(typeof(RoomTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoomTypeById(int id)
        {
            var response = await _roomTypeService.GetRoomTypeById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.RoomType.RoomTypeEndPoint)]
        [ProducesResponseType(typeof(RoomTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRoomType(int id, UpdateRoomTypeRequest request)
        {
            var response = await _roomTypeService.UpdateRoomType(id, request);
            return Ok(response);
        }

    }
}
