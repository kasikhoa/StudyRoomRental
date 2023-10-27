using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Room
    {
        public Room()
        {
            OrderItems = new HashSet<OrderItem>();
            RoomSchedules = new HashSet<RoomSchedule>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Facilities { get; set; } = null!;
        public string? Description { get; set; }
        public string? Area { get; set; }
        public int Capacity { get; set; }
        public string? Image { get; set; }
        public double CostPrice { get; set; }
        public string Status { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<RoomSchedule> RoomSchedules { get; set; }
    }
}
