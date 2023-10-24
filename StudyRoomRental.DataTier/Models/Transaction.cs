using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = null!;

        public virtual Payment Payment { get; set; } = null!;
    }
}
