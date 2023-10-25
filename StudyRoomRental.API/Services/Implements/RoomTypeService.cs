using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Payload.RoomType;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;
using StudyRoomRental.DataTier.Repository.Interfaces;

namespace StudyRoomRental.API.Services.Implements
{
    public class RoomTypeService : BaseService<RoomTypeService>, IRoomTypeService
    {

        public RoomTypeService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<RoomTypeService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<RoomTypeResponse> CreateRoomType(RoomTypeRequest request)
        {
            RoomType roomType = await _unitOfWork.GetRepository<RoomType>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(request.Name));
            if (roomType != null) throw new BadHttpRequestException(MessageConstant.RoomType.DuplicatedNameMessage);

            roomType = new RoomType()
            {
                Name = request.Name,
                Description = request.Description,
                Area = request.Area,
                MaxCapacity = request.MaxCapacity,
            };

            await _unitOfWork.GetRepository<RoomType>().InsertAsync(roomType);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.RoomType.CreateRoomTypeFailedMessage);

            return new RoomTypeResponse()
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                Area = roomType.Area,
                MaxCapacity = roomType.MaxCapacity,
            };
        }


        public async Task<IPaginate<RoomTypeResponse>> GetRoomTypes(string? searchName, int page, int size)
        {
            searchName = searchName?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<RoomTypeResponse> result = await _unitOfWork.GetRepository<RoomType>().GetPagingListAsync(
                selector: x => new RoomTypeResponse(x.Id, x.Name, x.Description, x.Area, x.MaxCapacity),
                predicate: string.IsNullOrEmpty(searchName) ? x => true : x => x.Name.Contains(searchName),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<RoomTypeResponse> GetRoomTypeById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.RoomType.EmptyIdMessage);
            RoomType roomType = await _unitOfWork.GetRepository<RoomType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (roomType == null) throw new BadHttpRequestException(MessageConstant.RoomType.NotFoundMessage);
            return new RoomTypeResponse(roomType.Id, roomType.Name, roomType.Description, roomType.Area, roomType.MaxCapacity);
        }

        public async Task<RoomTypeResponse> UpdateRoomType(int id, UpdateRoomTypeRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.RoomType.EmptyIdMessage);
            RoomType roomType = await _unitOfWork.GetRepository<RoomType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (roomType == null) throw new BadHttpRequestException(MessageConstant.RoomType.NotFoundMessage);

            request.TrimString();

            roomType.Name = string.IsNullOrEmpty(request.Name) ? roomType.Name : request.Name;
            roomType.Description = string.IsNullOrEmpty(request.Description) ? roomType.Description : request.Description;
            roomType.Area = string.IsNullOrEmpty(request.Area) ? roomType.Area : request.Area;
            roomType.MaxCapacity = (request.MaxCapacity <= 0) ? roomType.MaxCapacity : request.MaxCapacity;

            _unitOfWork.GetRepository<RoomType>().UpdateAsync(roomType);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.RoomType.UpdateFailedMessage);
            return new RoomTypeResponse(roomType.Id, roomType.Name, roomType.Description, roomType.Area, roomType.MaxCapacity);
        }
    }
}
