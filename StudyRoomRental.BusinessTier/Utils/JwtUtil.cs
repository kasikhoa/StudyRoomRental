using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudyRoomRental.DataTier.Models;
using Microsoft.IdentityModel.Tokens;

namespace StudyRoomRental.BusinessTier.Utils;

public class JwtUtil
{
    private JwtUtil()
    {

    }

    public static string GenerateJwtToken(Account account)
    {

        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        SymmetricSecurityKey secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StudyRoomRentalNumber1"));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, account.Role),
        };
        var expires = DateTime.Now.AddDays(10);
        var token = new JwtSecurityToken("StudyRoomRental", null, claims, notBefore: DateTime.Now, expires, credentials);
        return jwtHandler.WriteToken(token);
    }
}