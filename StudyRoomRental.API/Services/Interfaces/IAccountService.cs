using StudyRoomRental.BusinessTier.Payload.Login;

namespace StudyRoomRental.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}
