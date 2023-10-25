using Microsoft.EntityFrameworkCore;
using StudyRoomRental.API.Extensions;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Room;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;
using StudyRoomRental.DataTier.Repository.Interfaces;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace StudyRoomRental.API.Services.Implements
{
    public class RoomService : BaseService<RoomService>, IRoomService
    {

        public RoomService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<RoomService> logger) : base(unitOfWork, logger)
        {

        }     

        public async Task<RoomResponse> CreateNewRoom(CreateRoomRequest request)
        {

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.AccountId));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            if (!account.Role.Equals(RoleEnum.Landlord.GetDescriptionFromEnum()))
                throw new BadHttpRequestException(MessageConstant.Account.RenterRoleMessage);

            RoomType roomType = await _unitOfWork.GetRepository<RoomType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.RoomTypeId));
            if (roomType == null) throw new BadHttpRequestException(MessageConstant.RoomType.NotFoundMessage);


            Room newRoom = new Room()
            {
                AccountId = request.AccountId,
                RoomTypeId = request.RoomTypeId,
                Name = request.Name,
                Address = request.Address,
                Facilities = request.Facilities,
                Description = request.Description,
                Image = request.Image,
                CostPrice = request.CostPrice,
                Status = RoomStatus.Available.GetDescriptionFromEnum(),
            };
            await _unitOfWork.GetRepository<Room>().InsertAsync(newRoom);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Room.CreateRoomFailedMessage);
            return new RoomResponse()
            {
                Id = newRoom.Id,
                Account = account.Email,
                RoomType = roomType.Name,
                Name = newRoom.Name,
                Address = newRoom.Address,
                Facilities = newRoom.Facilities,
                Description = newRoom.Description,
                Image = newRoom.Image,
                CostPrice = newRoom.CostPrice,
                Status = EnumUtil.ParseEnum<RoomStatus>(newRoom.Status),
            };
        }

        private Expression<Func<Room, bool>> BuildGetRoomsQuery(int? accountId, int? roomTypeId, string? name, RoomStatus? status)
        {
            Expression<Func<Room, bool>> filterQuery = x => true;

            if (accountId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.AccountId.Equals(accountId));
            }

            if (roomTypeId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.RoomTypeId.Equals(roomTypeId));
            }
            if (!string.IsNullOrEmpty(name))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(name));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<RoomResponse>> ViewAllRooms(int? accountId, int? roomTypeId, string? name, RoomStatus? status, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<RoomResponse> result = await _unitOfWork.GetRepository<Room>().GetPagingListAsync(
                selector: x => new RoomResponse()
                {
                    Id = x.Id,
                    Account = x.Account.Email,
                    RoomType = x.RoomType.Name,
                    Name = x.Name,
                    Address= x.Address,
                    Facilities= x.Facilities,
                    Description = x.Description,
                    Image = x.Image,
                    CostPrice= x.CostPrice,
                    Status = EnumUtil.ParseEnum<RoomStatus>(x.Status)
                },
                predicate: BuildGetRoomsQuery(accountId, roomTypeId, name, status),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<RoomResponse> GetRoomById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Room.EmptyIdMessage);
            Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Account).Include(x => x.RoomType)
                );
            if (room == null) throw new BadHttpRequestException(MessageConstant.Room.NotFoundMessage);
            return new RoomResponse()
            {
                Id = room.Id,
                Account = room.Account.Email,
                RoomType = room.RoomType.Name,
                Name = room.Name,
                Address = room.Address,
                Facilities = room.Facilities,
                Description = room.Description,
                Image = room.Image,
                CostPrice = room.CostPrice,
                Status = EnumUtil.ParseEnum<RoomStatus>(room.Status)
            };
        }

        public async Task<RoomResponse> UpdateRoomInformation(int id, UpdateRoomRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Room.EmptyIdMessage);
            Room updateRoom = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Account).Include(x => x.RoomType)
                );
            if (updateRoom == null) throw new BadHttpRequestException(MessageConstant.Room.NotFoundMessage);
            request.TrimString();
            updateRoom.Name = string.IsNullOrEmpty(request.Name) ? updateRoom.Name : request.Name;
            updateRoom.Address = string.IsNullOrEmpty(request.Address) ? updateRoom.Address : request.Address;
            updateRoom.Facilities = string.IsNullOrEmpty(request.Facilities) ? updateRoom.Facilities : request.Facilities;
            updateRoom.Description = string.IsNullOrEmpty(request.Description) ? updateRoom.Description : request.Description;
            updateRoom.Image = string.IsNullOrEmpty(request.Image) ? updateRoom.Image : request.Image;
            updateRoom.CostPrice = request.CostPrice <= 0 ? updateRoom.CostPrice : request.CostPrice;
            updateRoom.Status = request.RoomStatus.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Room>().UpdateAsync(updateRoom);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Room.UpdateFailedMessage);
            return new RoomResponse()
            {
                Id = updateRoom.Id,
                Account = updateRoom.Account.Email,
                RoomType = updateRoom.RoomType.Name,
                Name = updateRoom.Name,
                Address = updateRoom.Address,
                Facilities = updateRoom.Facilities,
                Description = updateRoom.Description,
                Image = updateRoom.Image,
                CostPrice = updateRoom.CostPrice,
                Status = EnumUtil.ParseEnum<RoomStatus>(updateRoom.Status)
            };

        }

        public async Task<bool> UpdateRoomStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Room.EmptyIdMessage);
            Room room = await _unitOfWork.GetRepository<Room>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (room == null) throw new BadHttpRequestException(MessageConstant.Room.NotFoundMessage);

            room.Status = RoomStatus.Unavailable.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Room>().UpdateAsync(room);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
