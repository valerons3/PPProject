namespace SearchWork.Models.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Vacancy> vacanciesCategory { get; set; } = [];

        public ICollection<Vacancy> CategoryVacancies { get; set; } = [];
    }
}
