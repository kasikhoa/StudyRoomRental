    using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Room
{
    public class UpdateRoomRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Facilities { get; set; }
        public string? Description { get; set; }
        public string? Area { get; set; }
        public int Capacity { get; set; }
        public string? Image { get; set; }
        public double CostPrice { get; set; }
        public RoomStatus? Status { get; set; }

        public void TrimString()
        {
            Name = Name?.Trim();
            Address = Address?.Trim();
            Facilities= Facilities?.Trim();
            Description = Description?.Trim();
            Image = Image?.Trim();
        }
    }
}
