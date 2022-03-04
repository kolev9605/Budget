using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoriesRepository;

        public CategoryService(IRepository<Category> categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            var categories = await _categoriesRepository.AllAsync();

            var categoryModels = categories.Select(c => new CategoryModel()
            {
                Id = c.Id,
                Name = c.Name,
                CategoryType = c.CategoryType,
            });

            return categoryModels;
        }
    }
}
