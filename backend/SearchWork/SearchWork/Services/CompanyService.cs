using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SearchWork.Data;
using SearchWork.Models.DTO;
using SearchWork.Models.Entity;

namespace SearchWork.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext context;
        private readonly IValidator<CompanyCreateDTO> validator;

        public CompanyService(ApplicationDbContext context, IValidator<CompanyCreateDTO> validator)
        {
            this.context = context;
            this.validator = validator;
        }

        public async Task<CompanyCreateDTO?> FindCompanyByIdAsync(int userId)
        {
            var existingCompany = await context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingCompany == null) { return null; }

            CompanyCreateDTO company = new CompanyCreateDTO()
            {
                Name = existingCompany.Name,
                Description = existingCompany.Description,
                LogoPath = existingCompany.LogoPath,
                WebSite = existingCompany.Website
            };
            return company;
        }

        public async Task<(bool Success, string Message)> CreateCompany(int userId, CompanyCreateDTO companyDto)
        {
            var validationResult = await validator.ValidateAsync(companyDto);
            if (!validationResult.IsValid)
            {
                string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return (false, errors);
            }

            var existingCompany = await context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (existingCompany != null)
            {
                return (false, "Вы уже создали компанию");
            }

            Company company = new Company()
            {
                UserId = userId,
                Name = companyDto.Name,
                Description = companyDto.Description,
                LogoPath = companyDto.LogoPath,
                Website = companyDto.WebSite,
                CreatedAt = DateTime.UtcNow
            };

            context.Companies.Add(company);
            await context.SaveChangesAsync();

            return (true, "Компания успешно создана!");
        }
    }
}
