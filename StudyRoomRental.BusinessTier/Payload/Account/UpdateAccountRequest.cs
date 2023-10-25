using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Account
{
    public class UpdateAccountRequest
    {
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
    