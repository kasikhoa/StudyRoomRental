using StudyRoomRental.BusinessTier.Payload.RoomSchedule;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IRoomScheduleService
    {
        Task<RoomScheduleResponse> CreateRoomSchedule(RoomScheduleRequest request);
    }
}
