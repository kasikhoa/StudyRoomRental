using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Order
    {
        public Order()
        {
            Feedbacks = new HashSet<Feedback>();
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }

        public virtual Account User { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
