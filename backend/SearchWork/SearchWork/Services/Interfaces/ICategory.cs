﻿using SearchWork.Models.DTO;

namespace SearchWork.Services.Interfaces
{
    public interface ICategory
    {
        public Task<(bool, string)> AddCategoryRequestAsync(CategoryDTO model);
        public Task<List<CategoryDTO>?> GetAllNotAddedCategoryRequestAsync();
        public Task<bool> ConfirmationAddingCategoryAsync(CategoryDTO model);
        public Task<bool> DeleteAllAddedCategoryRequestAsync();
        public Task<List<CategoryDTO>?> GetAllCategoryAsync();
        public Task<(bool, string)> DeleteCategoryAsync(CategoryDTO model);
        public Task<int?> GetIdCategoryByNameAsync(string name);
        public Task<List<VacancyDTO>?> GetAllVacancyByCategoryAsync(string name);
    }
}
