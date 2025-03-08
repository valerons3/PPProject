using SearchWork.Models.DTO;

namespace SearchWork.Services
{
    public interface ICategory
    {
        public Task<(bool, string)> AddCategoryRequestAsync(CategoryDTO model);
        public Task<List<CategoryDTO>?> GetAllNotAddedCategoryRequestAsync();
        public Task<bool> ConfirmationAddingCategoryAsync(CategoryDTO model);
        public Task<bool> DeleteAllAddedCategoryRequestAsync();
        public Task<List<CategoryDTO>?> GetAllCategoryAsync();
        public Task<(bool, string)> DeleteCategoryAsync(CategoryDTO model);
    }
}
