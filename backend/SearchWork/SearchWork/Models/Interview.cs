namespace SearchWork.Models
{
    public class Interview
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
