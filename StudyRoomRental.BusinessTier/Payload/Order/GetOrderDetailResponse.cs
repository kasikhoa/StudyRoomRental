using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Order
{
    public class GetOrderDetailResponse
    {
        public Guid Id { get; set; }
        public string? Account { get; set; }
        public DateTime CreatedTime { get; set; }
        public int RoomQuantity { get; set; }
        public double TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemResponse> RoomList { get; set; } = new List<OrderItemResponse>();
    }

    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double CostPrice { get; set; }
    }
}
