using System;

namespace SearchWork.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public int CategoryId { get; set; }
        public JobCategory Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
