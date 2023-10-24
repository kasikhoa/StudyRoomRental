using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Room
    {
        public Room()
        {
            OrderItems = new HashSet<OrderItem>();
            RoomActivities = new HashSet<RoomActivity>();
        }

        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public string Address { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public string Status { get; set; } = null!;

        public virtual RoomType Type { get; set; } = null!;
        public virtual Account User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<RoomActivity> RoomActivities { get; set; }
    }
}
