using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class JobCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Job> Jobs { get; set; } = new();
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}
