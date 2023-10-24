using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime EndedTime { get; set; }
        public string Status { get; set; }

        public virtual Order Order { get; set; }
        public virtual Room Room { get; set; }
    }
}
