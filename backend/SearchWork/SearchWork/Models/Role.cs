using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
