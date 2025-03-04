namespace SearchWork.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public User Seeker { get; set; } = null!;
        public int CategoryId { get; set; }
        public JobCategory Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
