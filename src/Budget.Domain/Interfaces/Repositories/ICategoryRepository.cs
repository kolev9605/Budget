using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Records.Create;

namespace Budget.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<CategoryModel>> GetAllAsync(string userId);

    Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId);

    Task<Category?> GetByIdWithSubcategoriesAsync(Guid categoryId, string userId);

    Task<CategoryModel?> GetByIdWithSubcategoriesMappedAsync(Guid categoryId, string userId);

    Task<Category?> GetByNameWithUsersAsync(string name);

    Task<Category?> GetForDeletionAsync(Guid categoryId, string userId);

    Task<IEnumerable<Category>> GetInitialCategoriesAsync();

    Task<IEnumerable<CategoryModel>> GetSubcategoriesByParentCategoryIdMappedAsync(Guid parentCategoryId, string userId);

    Task<CategoryForRecordCreationModel?> GetForRecordCreationAsync(Guid id);

    Task<IEnumerable<GetMostUsedCategoriesResult>> GetMostUsedCategoriesAsync(string userId, int count, int recordsCount = 1000);
}
