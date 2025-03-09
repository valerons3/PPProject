using SearchWork.Models.DTO;
using SearchWork.Services.Interfaces;
using SearchWork.Data;
using Microsoft.EntityFrameworkCore;
using SearchWork.Models.Entity;

namespace SearchWork.Services
{
    public class VacancyService : IVacancy
    {
        private ApplicationDbContext context;
        private ICategory categoryService;
        public VacancyService(ApplicationDbContext context, ICategory categoryService)
        {
            this.context = context;
            this.categoryService = categoryService;
        }
        public async Task<(bool Success, string Message)> CreateVacancyAsync(VacancyDTO model, int userId)
        {
            var company = await context.Companies
                                .Where(c => c.UserId == userId)
                                .FirstOrDefaultAsync();
            if (company == null) { return (false, "Для создания вакансий сначала нужно создать компанию"); }

            int? categoryId = await categoryService.GetIdCategoryByNameAsync(model.CategoryName);
            if (categoryId == null) { return (false, "Такой категории не существует"); }


            Vacancy vacancy = new Vacancy()
            {
                CompanyId = company.CompanyId,
                CategoryId = categoryId.Value,
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                Salary = model.Salary,
                CreatedAt = DateTime.UtcNow
            };

            context.Vacancies.Add(vacancy);
            await context.SaveChangesAsync();
            return (true, $"Вакансия: {model.Title} успешно создана");
        }

        public Task<(bool Success, string Message)> DeleteVacancyByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<VacancyDTO>?> GetAllVacanciesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<VacancyDTO>?> GetAllVacanciesByCategoryAsync(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task<List<VacancyDTO>?> GetAllVacanciesCompanyAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
