namespace SearchWork.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public User Seeker { get; set; } = null!;
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
