using StudyRoomRental.BusinessTier.Payload.Account;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.DataTier.Models;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);

        Task<GetAccountResponse> CreateNewAccount(AccountRequest request);
    }
}
