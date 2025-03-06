namespace SearchWork.Models.Entity
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public int VacansyId { get; set; }
        public string Status { get; set; }
        public string CoverLetter { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public Interview? interview { get; set; }
    }
}
