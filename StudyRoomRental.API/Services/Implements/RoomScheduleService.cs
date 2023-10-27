using Microsoft.EntityFrameworkCore;
using StudyRoomRental.API.Extensions;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.RoomSchedule;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;
using StudyRoomRental.DataTier.Repository.Interfaces;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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
                StartTime = TimeUtils.ConvertToSEATime(request.StartTime),
                EndTime = TimeUtils.ConvertToSEATime(request.EndTime),
                Note = request.Note,
                Status = RoomScheduleStatus.Pending.GetDescriptionFromEnum()
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

        private Expression<Func<RoomSchedule, bool>> BuildGetSchedulesQuery(int? roomId, DateTime? startTime, DateTime? endTime, RoomScheduleStatus? status)
        {
            Expression<Func<RoomSchedule, bool>> filterQuery = x => true;

            if (roomId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.RoomId == roomId);
            }

            if (startTime.HasValue)
            {
                var startTimeWithoutSeconds = startTime.Value.AddSeconds(-startTime.Value.Second).AddMilliseconds(-startTime.Value.Millisecond);
                filterQuery = filterQuery.AndAlso(x => x.StartTime >= startTimeWithoutSeconds);
            }

            if (endTime.HasValue)
            {
                var endTimeWithoutSeconds = endTime.Value.AddSeconds(-endTime.Value.Second).AddMilliseconds(-endTime.Value.Millisecond);
                filterQuery = filterQuery.AndAlso(x => x.EndTime <= endTimeWithoutSeconds);
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }


        public async Task<IPaginate<RoomScheduleResponse>> ViewAllRoomSchedules(int? roomId, DateTime? startTime, DateTime? endTime,
            RoomScheduleStatus? status, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<RoomScheduleResponse> result = await _unitOfWork.GetRepository<RoomSchedule>().GetPagingListAsync(
                selector: x => new RoomScheduleResponse()
                {
                    Id = x.Id,
                    RoomName = x.Room.Name,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Note = x.Note,
                    Status = EnumUtil.ParseEnum<RoomScheduleStatus>(x.Status)
                },
                predicate: BuildGetSchedulesQuery(roomId, startTime, endTime, status));
            return result;

        }

        public async Task<RoomScheduleResponse> UpdateRoomSchedule(int id, UpdateRoomScheduleRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.RoomSchedule.EmptyIdMessage);

            RoomSchedule roomSchedule = await _unitOfWork.GetRepository<RoomSchedule>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Room)
                );
            if (roomSchedule == null) throw new BadHttpRequestException(MessageConstant.RoomSchedule.NotFoundMessage);

            roomSchedule.StartTime = request.StartTime; 
            roomSchedule.EndTime = request.EndTime;
            roomSchedule.Note = string.IsNullOrEmpty(request.Note) ? roomSchedule.Note : request.Note;
            roomSchedule.Status = request.Status.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<RoomSchedule>().UpdateAsync(roomSchedule);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.RoomSchedule.UpdateFailedMessage);

            return new RoomScheduleResponse()
            {
                Id = roomSchedule.Id,
                RoomName = roomSchedule.Room.Name,
                StartTime = roomSchedule.StartTime,
                EndTime = roomSchedule.EndTime,
                Note = roomSchedule.Note,
                Status = EnumUtil.ParseEnum<RoomScheduleStatus>(roomSchedule.Status)
            };

        }

        public async Task<RoomScheduleResponse> GetRoomScheduleById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.RoomSchedule.EmptyIdMessage);

            RoomSchedule roomSchedule = await _unitOfWork.GetRepository<RoomSchedule>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Room)
                );
            if (roomSchedule == null) throw new BadHttpRequestException(MessageConstant.RoomSchedule.NotFoundMessage);

            return new RoomScheduleResponse()
            {
                Id = roomSchedule.Id,
                RoomName = roomSchedule.Room.Name,
                StartTime = roomSchedule.StartTime,
                EndTime = roomSchedule.EndTime,
                Note = roomSchedule.Note,
                Status = EnumUtil.ParseEnum<RoomScheduleStatus>(roomSchedule.Status)
            };
        }
    }
}
