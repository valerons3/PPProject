﻿namespace SearchWork.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty; // "seeker" or "employer"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
