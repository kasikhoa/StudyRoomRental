using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Repository.Interfaces;
using System;
using System.Linq.Expressions;

namespace StudyRoomRental.API.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<StudyRoomRentalContext> unitOfWork, ILogger<AccountService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                p.Email.Equals(loginRequest.Email) &&
                p.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));

            Account account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: searchFilter);
            if (account == null) return null;

            var token = JwtUtil.GenerateJwtToken(account);

            LoginResponse loginResponse = new LoginResponse()
            {
                AccessToken = token,
                Id = account.Id,
                Email = account.Email,
                Role = EnumUtil.ParseEnum<RoleEnum>(account.Role),
                Status = EnumUtil.ParseEnum<AccountStatus>(account.Status),
            };
            return loginResponse;
        }
    }
}
