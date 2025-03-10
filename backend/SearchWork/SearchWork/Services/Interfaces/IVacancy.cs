﻿using SearchWork.Models.DTO;

namespace SearchWork.Services.Interfaces
{
    public interface IVacancy
    {
        public Task<(bool Success, string Message)> CreateVacancyAsync(VacancyDTO model, int userId);
        public Task<List<VacancyDTO>?> GetAllVacanciesCompanyAsync(string name);
        public Task<List<VacancyDTO>?> GetAllVacanciesAsync();
        public Task<(bool Success, string Message)> DeleteVacancyByNameAsync(string name);
    }
}
