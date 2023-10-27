using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double CostPrice { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
    }
}
