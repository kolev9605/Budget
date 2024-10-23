using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Records.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<CategoryModel>> GetAllPrimaryCategoryModelsAsync(string userId);

    Task<IEnumerable<CategoryModel>> GetAllWithSubcategoriesCategoryModelsAsync(string userId);

    Task<Category?> GetByIdWithSubcategoriesAsync(Guid categoryId, string userId);

    Task<CategoryModel?> GetByIdWithSubcategoriesMappedAsync(Guid categoryId, string userId);

    Task<Category?> GetByNameWithUsersAsync(string name);

    Task<Category?> GetForDeletionAsync(Guid categoryId, string userId);

    Task<IEnumerable<Category>> GetInitialCategoriesAsync();

    Task<IEnumerable<CategoryModel>> GetSubcategoriesByParentCategoryIdMappedAsync(Guid parentCategoryId, string userId);

    Task<CategoryForRecordCreationModel?> GetForRecordCreationAsync(Guid id);
}
