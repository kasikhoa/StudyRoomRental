using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public string Rating { get; set; } = null!;
        public string? Content { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
