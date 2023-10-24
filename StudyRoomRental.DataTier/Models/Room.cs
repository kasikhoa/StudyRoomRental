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
        public string Address { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }

        public virtual RoomType Type { get; set; }
        public virtual Account User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<RoomActivity> RoomActivities { get; set; }
    }
}
