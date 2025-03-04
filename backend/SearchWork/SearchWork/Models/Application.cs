namespace SearchWork.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public User Seeker { get; set; } = null!;
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public string Status { get; set; } = "pending";
        public string? CoverLetter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
