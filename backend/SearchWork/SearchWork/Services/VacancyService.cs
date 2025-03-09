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

        public async Task<List<VacancyDTO>?> GetAllVacanciesAsync()
        {
            var vacancies = await context.Vacancies.ToListAsync();
            if (vacancies.Count == 0) { return null; }

            var vacancyDTOs = vacancies.Select(v => new VacancyDTO
            {
                Title = v.Title,
                Description = v.Description,
                Location = v.Location,
                Salary = v.Salary,
            }).ToList();

            return vacancyDTOs;
        }


        public Task<List<VacancyDTO>?> GetAllVacanciesCompanyAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
