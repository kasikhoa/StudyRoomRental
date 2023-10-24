using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class RoomActivity
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime? EndedTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }

        public virtual Room Room { get; set; }
        public virtual Account User { get; set; }
    }
}
