using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Rating { get; set; } = null!;
        public string? Content { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Account User { get; set; } = null!;
    }
}
