using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class RoomSchedule
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = null!;

        public virtual Room Room { get; set; } = null!;
    }
}
