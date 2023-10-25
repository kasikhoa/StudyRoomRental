using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Room
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string RoomType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Facilities { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double CostPrice { get; set; }
        public RoomStatus Status { get; set; }

        public RoomResponse() { }
    }
}
