using StudyRoomRental.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Order
{
    public class UpdateOrderRequest
    {
        public OrderStatus Status { get; set; }
    }
}
