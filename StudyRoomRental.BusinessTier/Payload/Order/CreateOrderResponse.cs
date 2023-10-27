using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Order
{
    public class CreateOrderResponse
    {
        public Guid Id { get; set; }
        public string Account { get; set; } 
        public DateTime CreatedTime { get; set; }
        public int RoomQuantity { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
