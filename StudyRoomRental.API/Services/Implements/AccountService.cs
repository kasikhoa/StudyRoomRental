using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Account;
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

        public async Task<GetAccountResponse> CreateNewAccount(AccountRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
              .SingleOrDefaultAsync(predicate: x => x.Email.Equals(request.Email));
            if (account != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            account = new Account()
            {
                Email = request.Email,
                Password = PasswordUtil.HashPassword(request.Password),
                Role = RoleEnum.Renter.GetDescriptionFromEnum(),
                Status = AccountStatus.Activate.GetDescriptionFromEnum(),
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Account.CreateAccountFailed);
            return new GetAccountResponse(account.Id, account.Email, account.Password, EnumUtil.ParseEnum<RoleEnum>(account.Role), EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

    }
}
