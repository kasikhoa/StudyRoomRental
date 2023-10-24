using StudyRoomRental.BusinessTier.Enums;

namespace StudyRoomRental.BusinessTier.Payload.Login;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public int Id { get; set; }
    public string Email { get; set; }
    public RoleEnum Role { get; set; }    
    public AccountStatus Status { get; set; }

    public LoginResponse() 
    {
    }

    public LoginResponse(string accessToken, int id, string email, RoleEnum role, AccountStatus status)
    {
        AccessToken = accessToken;
        Id = id;
        Email = email;
        Role = role;
        Status = status;
    }
}

