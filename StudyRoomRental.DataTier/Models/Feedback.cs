using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
