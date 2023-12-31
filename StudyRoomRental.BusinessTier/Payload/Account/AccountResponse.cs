﻿using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Account
{
    public class AccountResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleEnum Role { get; set; }
        public AccountStatus Status { get; set; }
    }
}
