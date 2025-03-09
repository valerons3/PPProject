namespace SearchWork.Models.DTO
{
    public class VacancyDTO
    {
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Salary { get; set; }
    }
}
