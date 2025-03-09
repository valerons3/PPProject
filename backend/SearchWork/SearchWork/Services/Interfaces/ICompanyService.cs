using SearchWork.Models.DTO;

namespace SearchWork.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<(bool Success, string Message)> CreateCompany(int userId, CompanyCreateDTO companyDto);
        public Task<CompanyCreateDTO?> FindCompanyByIdAsync(int userId);
    }
}
