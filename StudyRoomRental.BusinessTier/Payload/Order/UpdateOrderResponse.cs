using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Order
{
    public class UpdateOrderResponse
    {
        public DateTime CreatedTime { get; set; }
        public int RoomQuantity { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string Message { get; set; }

        public UpdateOrderResponse()
        {
        }

        public UpdateOrderResponse(DateTime createdTime, int roomQuantity, double totalAmount, OrderStatus status, DateTime? completedTime, string message)
        {
            CreatedTime = createdTime;
            RoomQuantity = roomQuantity;
            TotalAmount = totalAmount;
            Status = status;
            CompletedTime = completedTime;
            Message = message;
        }
    }
}
