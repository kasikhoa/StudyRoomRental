using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public string Area { get; set; } = null!;
        public int MaxCapacity { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
