using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SearchWork.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int CategoryId { get; set; }
        public JobCategory Category { get; set; }

        public string Skills { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "open";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Application> Applications { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }
}
