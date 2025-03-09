namespace SearchWork.Models.DTO
{
    public class CreateVacancyDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Salary { get; set; }
    }

}
