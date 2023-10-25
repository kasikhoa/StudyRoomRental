using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Account
    {
        public Account()
        {
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
