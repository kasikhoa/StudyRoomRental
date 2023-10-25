using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Account;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.DataTier.Models;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);

        Task<GetAccountResponse> CreateNewAccount(AccountRequest request);

        Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest);

        Task<GetAccountResponse> GetAccountDetail(int id);

        Task<bool> UpdateAccountStatus(int id);

        Task<bool> UpdateAccountRole(int id);

        Task<IPaginate<GetAccountResponse>> GetAccounts(string? searchEmail, RoleEnum? role, AccountStatus? status, int page, int size);

    }
}
