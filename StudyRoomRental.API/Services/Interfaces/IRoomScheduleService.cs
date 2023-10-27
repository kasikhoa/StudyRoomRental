using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.RoomSchedule;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IRoomScheduleService
    {
        Task<RoomScheduleResponse> CreateRoomSchedule(RoomScheduleRequest request);
        Task<IPaginate<RoomScheduleResponse>> ViewAllRoomSchedules(int? roomId, DateTime? startTime, DateTime? endTime,
            RoomScheduleStatus? Status, int page, int size);
        Task<RoomScheduleResponse> UpdateRoomSchedule(int id, UpdateRoomScheduleRequest request);
        Task<RoomScheduleResponse> GetRoomScheduleById(int id);
    }
}
