using System.ComponentModel.DataAnnotations;

namespace StudyRoomRental.BusinessTier.Payload.Login;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is missing")]
    [MaxLength(50, ErrorMessage = "Username's max length is 50 characters")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is missing")]
    [MaxLength(64, ErrorMessage = "Password's max length is 64 characters")]
    public string Password { get; set; }
}