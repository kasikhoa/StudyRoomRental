using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Order
{
    public class CreateOrderRequest
    {
        public int AccountId { get; set; }
        public List<OrderRoom> RoomList { get; set; } = new List<OrderRoom>();
        public double TotalAmount { get; set; }
    }

    public class OrderRoom
    {
        public int RoomId { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double CostPrice { get; set; }

    }
}
