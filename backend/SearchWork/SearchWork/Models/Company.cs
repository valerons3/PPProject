using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Job> Jobs { get; set; } = new();
    }
}
