using StudyRoomRental.BusinessTier.Payload.RoomType;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<RoomTypeResponse> CreateRoomType(RoomTypeRequest request);
        Task<IPaginate<RoomTypeResponse>> GetRoomTypes(string? searchName, int page, int size);
        Task<RoomTypeResponse> GetRoomTypeById(int id);
        Task<RoomTypeResponse> UpdateRoomType(int id, UpdateRoomTypeRequest request);
    }
}
