using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SearchWork.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Application> Applications { get; set; } = new();
        public List<Resume> Resumes { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}
