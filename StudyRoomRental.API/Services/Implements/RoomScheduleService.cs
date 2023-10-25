using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.RoomSchedule;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Repository.Interfaces;

namespace StudyRoomRental.API.Services.Implements
{
    public class RoomScheduleService : BaseService<RoomScheduleService>, IRoomScheduleService
    {

        public RoomScheduleService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<RoomScheduleService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<RoomScheduleResponse> CreateRoomSchedule(RoomScheduleRequest request)
        {
            Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.RoomId));
            if (room == null) throw new BadHttpRequestException(MessageConstant.Room.NotFoundMessage);


            RoomSchedule roomSchedule = new RoomSchedule()
            {
                RoomId = request.RoomId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Note = request.Note,
                Status = request.Status.GetDescriptionFromEnum()
            };
            await _unitOfWork.GetRepository<RoomSchedule>().InsertAsync(roomSchedule);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.RoomSchedule.CreateFailedMessage);
            return new RoomScheduleResponse()
            {
                Id = roomSchedule.Id,
                RoomName = room.Name,
                StartTime = roomSchedule.StartTime,
                EndTime = roomSchedule.EndTime,
                Note = roomSchedule.Note,
                Status = EnumUtil.ParseEnum<RoomScheduleStatus>(roomSchedule.Status)

            };
        }
    }
}
