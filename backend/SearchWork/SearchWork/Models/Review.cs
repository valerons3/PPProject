using System;
using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
