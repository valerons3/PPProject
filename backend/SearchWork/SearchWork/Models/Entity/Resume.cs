namespace SearchWork.Models.Entity
{
    public class Resume
    {
        public int ResumeId { get; set; }
        public int UserId { get; set; }
        public string ResumePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
