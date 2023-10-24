using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Account
{
    public class AccountRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
