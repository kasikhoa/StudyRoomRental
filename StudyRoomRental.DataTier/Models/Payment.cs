using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
