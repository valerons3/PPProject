namespace SearchWork.Models.Entity
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ReviewerId { get; set; }
        public int ReviewedId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public User? Reviewer { get; set; }
        public User? Reviewed { get; set; }

    }
}
