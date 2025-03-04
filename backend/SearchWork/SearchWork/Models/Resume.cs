namespace SearchWork.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public User Seeker { get; set; } = null!;
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
