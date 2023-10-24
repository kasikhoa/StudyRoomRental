using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Account
{
    public class GetAccountResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
        public AccountStatus Status { get; set; }

        public GetAccountResponse(int id, string email, string password, RoleEnum role, AccountStatus status)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
            Status = status;
        }
    }
}
