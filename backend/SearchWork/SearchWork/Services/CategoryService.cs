using Microsoft.EntityFrameworkCore;
using SearchWork.Data;
using SearchWork.Models.DTO;
using SearchWork.Models.Entity;

namespace SearchWork.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<(bool, string)> AddCategoryRequestAsync(CategoryDTO model)
        {
            if (string.IsNullOrEmpty(model.CategoryName))
            {
                return (false, "Название категории не должно быть пустым");
            }

            if (await context.Categories.AnyAsync(c => c.Name == model.CategoryName))
            {
                return (false, "Такая категория уже есть в списке категорий");
            }

            if (await context.CategoryRequests.AnyAsync(c => c.Name == model.CategoryName))
            {
                return (false, "Запрос на добавление этой категории уже существует");
            }

            CategoryRequest categoryRequest = new CategoryRequest()
            {
                Name = model.CategoryName,
                IsAdded = false,
                CreatedAt = DateTime.UtcNow
            };

            context.CategoryRequests.Add(categoryRequest);
            await context.SaveChangesAsync();

            return (true, $"Запрос на добавление категории \"{model.CategoryName}\" успешно создан");
        }

        public async Task<List<CategoryDTO>?> GetAllNotAddedCategoryRequestAsync()
        {
            var pendingCategories = await context.CategoryRequests
                .Where(c => !c.IsAdded)
                .Select(c => new CategoryDTO
                {
                    CategoryName = c.Name
                })
                .ToListAsync();
            if (pendingCategories.Count == 0) { return null; }

            return pendingCategories;
        }

        public async Task<bool> ConfirmationAddingCategoryAsync(CategoryDTO model)
        {
            if (!await context.CategoryRequests.AnyAsync(c => c.Name == model.CategoryName))
            {
                return false;
            }
            var category = new Category()
            {
                Name= model.CategoryName
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            CategoryRequest categoryRequest =
                await context.CategoryRequests.FirstOrDefaultAsync(c => c.Name == model.CategoryName);

            categoryRequest.IsAdded = true;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllAddedCategoryRequestAsync()
        {
            var categoriesRequest = await context.CategoryRequests
                .Where(c => c.IsAdded == true)
                .ToListAsync();

            if (categoriesRequest.Count == 0) { return false; }

            context.CategoryRequests.RemoveRange(categoriesRequest);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CategoryDTO>?> GetAllCategoryAsync()
        {
            var categories = await context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryName= c.Name
                }).ToListAsync();

            if (categories.Count == 0) { return null; }

            return categories;
        }

        public async Task<(bool, string)> DeleteCategoryAsync(CategoryDTO model)
        {
            if (string.IsNullOrEmpty(model.CategoryName))
            {
                return (false, "Название категории не должно быть пустым");
            }

            if (!await context.CategoryRequests.AnyAsync(c => c.Name == model.CategoryName))
            {
                return (false, "Такой категории не существует");
            }

            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == model.CategoryName);
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return (true, $"Категория \"{model.CategoryName}\" успешно удалена");
        }
    }
}
