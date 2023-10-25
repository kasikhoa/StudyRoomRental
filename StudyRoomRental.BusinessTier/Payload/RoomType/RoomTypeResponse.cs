using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.RoomType
{
    public class RoomTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public int MaxCapacity { get; set; }

        public RoomTypeResponse() { }

        public RoomTypeResponse(int id, string name, string description, string area, int maxCapacity)
        {
            Id = id;
            Name = name;
            Description = description;
            Area = area;
            MaxCapacity = maxCapacity;
        }
    }
}
