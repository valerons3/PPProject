namespace SearchWork.Models.Entity
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Application? Application { get; set; }
    }
}
