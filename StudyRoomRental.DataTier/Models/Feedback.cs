using System;
using System.Collections.Generic;

namespace StudyRoomRental.DataTier.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Rating { get; set; }
        public string Content { get; set; }

        public virtual Order Order { get; set; }
        public virtual Account User { get; set; }
    }
}
