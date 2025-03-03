using System;
using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class Application
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "pending";

        public string CoverLetter { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
