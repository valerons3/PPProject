using System;

namespace SearchWork.Models
{
    public class Resume
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public string FilePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
