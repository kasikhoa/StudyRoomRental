using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Feedback
{
    public class FeedBackRequest
    {
        public double Rating { get; set; }
        public string? Content { get; set; }
    }
}
