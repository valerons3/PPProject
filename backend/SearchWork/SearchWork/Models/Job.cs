namespace SearchWork.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public int CategoryId { get; set; }
        public JobCategory Category { get; set; } = null!;
        public string? Skills { get; set; }
        public string Status { get; set; } = "open";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
