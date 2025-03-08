namespace SearchWork.Models.Entity
{
    public class CategoryRequest
    {
        public int RequestId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAdded { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
