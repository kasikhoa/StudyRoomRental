using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyRoomRental.BusinessTier.Payload.Feedback
{
    public class FeedBackResponse
    {
        public int Id { get; set; } 
        public double Rating { get; set; }
        public string? Content { get; set; }

        public FeedBackResponse(int id, double rating, string? content)
        {
            Id = id;
            Rating = rating;
            Content = content;
        }
    }
}
