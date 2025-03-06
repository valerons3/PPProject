using System.Collections;

namespace SearchWork.Models.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
        public Resume? UserResume { get; set; }
        public Role? UserRole { get; set; }
        public ICollection<Application> UserApplications { get; set; } = [];
        public ICollection<Notification> UserNotifications { get; set; } = [];
        public ICollection<Review> UserReviews { get; set; } = [];
        public ICollection<Review> GivenReviews { get; set; } = [];
        public ICollection<Review> ReceivedReviews { get; set; } = [];

    }
}
