using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Room;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponse> CreateNewRoom(CreateRoomRequest request); 
        Task<IPaginate<RoomResponse>> ViewAllRooms(int? accountId, int? roomTypeId, string? name, RoomStatus? status, int page, int size);
        Task<RoomResponse> GetRoomById(int id);
        Task<RoomResponse> UpdateRoomInformation(int id, UpdateRoomRequest request);
        Task<bool> UpdateRoomStatus(int id);
    }
}
