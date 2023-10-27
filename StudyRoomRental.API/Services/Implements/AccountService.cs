using StudyRoomRental.API.Extensions;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Account;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.BusinessTier.Utils;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;
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
                Role = request.Role.GetDescriptionFromEnum(),
                Status = AccountStatus.Activate.GetDescriptionFromEnum(),
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Account.CreateAccountFailed);
            return new GetAccountResponse(account.Id, account.Email, account.Password, EnumUtil.ParseEnum<RoleEnum>(account.Role), EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

        public async Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account updateAccount = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (updateAccount == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            updateAccount.Password = string.IsNullOrEmpty(updateAccountRequest.Password) ? updateAccount.Password : PasswordUtil.HashPassword(updateAccountRequest.Password.Trim());
            updateAccount.Phone = string.IsNullOrEmpty(updateAccountRequest.Phone) ? updateAccount.Phone : updateAccountRequest.Phone.Trim();
            updateAccount.DateOfBirth = string.IsNullOrEmpty(updateAccountRequest.DateOfBirth) ? updateAccount.DateOfBirth : updateAccountRequest.DateOfBirth.Trim();
            updateAccount.Gender = updateAccountRequest.Gender.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Account>().UpdateAsync(updateAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<GetAccountResponse> GetAccountDetail(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);
            return new GetAccountResponse(account.Id, account.Email, account.Password,
                EnumUtil.ParseEnum<RoleEnum>(account.Role), EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

        public async Task<bool> UpdateAccountStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            account.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
            return isSuccessfull;
        }

        public async Task<bool> UpdateAccountRole(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);
            account.Role = RoleEnum.Landlord.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
            return isSuccessfull;
        }

        public async Task<IPaginate<GetAccountResponse>> GetAccounts(string? searchEmail, RoleEnum? role, AccountStatus? status, int page, int size)
        {
            searchEmail = searchEmail?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<GetAccountResponse> accounts = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: x => new GetAccountResponse(x.Id, x.Email, x.Password, EnumUtil.ParseEnum<RoleEnum>(x.Role), EnumUtil.ParseEnum<AccountStatus>(x.Status)),
                predicate: BuildGetAccountsQuery(searchEmail, role, status),
                orderBy: x => x.OrderBy(x => x.Id),
                page: page,
                size: size
                );
            return accounts;
        }

        private Expression<Func<Account, bool>> BuildGetAccountsQuery(string? searchEmail, RoleEnum? role, AccountStatus? status)
        {
            Expression<Func<Account, bool>> filterQuery = x => true;

            if (!string.IsNullOrEmpty(searchEmail))
            {
                filterQuery = filterQuery.AndAlso(x => x.Email.Contains(searchEmail));
            } 

            if (role != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Role == role.GetDescriptionFromEnum());
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status == status.GetDescriptionFromEnum());
            }

            return filterQuery;
        }
    }
}
