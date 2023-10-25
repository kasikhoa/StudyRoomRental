using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.RoomType
{
    public class RoomTypeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public int MaxCapacity { get; set; }
    }
}
