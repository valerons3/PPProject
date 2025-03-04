namespace SearchWork.Models
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
