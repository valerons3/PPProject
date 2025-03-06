namespace SearchWork.Models.Entity
{
    public class Vacancy
    {
        public int VacancyId { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Salary { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }
}
