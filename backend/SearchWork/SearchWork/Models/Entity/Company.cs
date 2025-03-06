using System.Collections;

namespace SearchWork.Models.Entity
{
    public class Company
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public ICollection<Vacancy> CompanyVacancies { get; set; } = [];
    }
}
