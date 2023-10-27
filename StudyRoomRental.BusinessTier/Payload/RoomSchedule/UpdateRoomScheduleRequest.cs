using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.RoomSchedule
{
    public class UpdateRoomScheduleRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set;}
        public string? Note { get; set; }
        public RoomScheduleStatus Status { get; set; }

    }
}
