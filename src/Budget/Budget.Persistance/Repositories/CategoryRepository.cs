using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(BudgetDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<CategoryModel>> GetAllPrimaryCategoryModelsAsync(string userId)
    {
        var categories = await GetUserCategories(userId)
            .Include(c => c.SubCategories)
            .Where(c => !c.ParentCategoryId.HasValue)
            .OrderBy(c => c.ParentCategoryId ?? c.Id)
            .ThenBy(c => c.Id)
            .ProjectToType<CategoryModel>()
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<CategoryModel>> GetAllWithSubcategoriesCategoryModelsAsync(string userId)
    {
        var categories = await GetUserCategories(userId)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.ParentCategoryId ?? c.Id)
            .ThenBy(c => c.Id)
            .ProjectToType<CategoryModel>()
            .ToListAsync();

        return categories;
    }

    public async Task<Category?> GetByIdWithSubcategoriesAsync(Guid categoryId, string userId)
    {
        var category = await GetByIdWithSubcategoriesBaseQuery(userId, categoryId)
            .FirstOrDefaultAsync();

        return category;
    }

    public async Task<CategoryModel?> GetByIdWithSubcategoriesMappedAsync(Guid categoryId, string userId)
    {
        var category = await GetByIdWithSubcategoriesBaseQuery(userId, categoryId)
            .ProjectToType<CategoryModel>()
            .FirstOrDefaultAsync();

        return category;
    }

    public async Task<Category?> GetByNameWithUsersAsync(string name)
    {
        var category = await _budgetDbContext.Categories
            .Include(c => c.Users)
            .Where(c => c.Name == name)
            .FirstOrDefaultAsync();

        return category;
    }

    public async Task<Category?> GetForDeletionAsync(Guid categoryId, string userId)
    {
        var categories = await GetUserCategories(userId)
            .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Records)
            .Include(c => c.Records)
            .Include(c => c.Users)
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync();

        return categories;
    }

    public async Task<IEnumerable<Category>> GetInitialCategoriesAsync()
    {
        var categories = await _budgetDbContext.Categories
            .Where(c => c.IsInitial)
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<CategoryModel>> GetSubcategoriesByParentCategoryIdMappedAsync(Guid parentCategoryId, string userId)
    {
        var subcategories = await GetUserCategories(userId)
            .Include(c => c.ParentCategory)
            .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId == parentCategoryId)
            .ProjectToType<CategoryModel>()
            .ToListAsync();

        return subcategories;
    }

    private IQueryable<Category> GetUserCategories(string userId)
    {
        var categories = _budgetDbContext.Categories
            .Include(c => c.Users)
            .Where(c => c.Users.Where(u => u.UserId == userId).Any());

        return categories;
    }

    private IQueryable<Category> GetByIdWithSubcategoriesBaseQuery(string userId, Guid categoryId)
    {
        return GetUserCategories(userId)
            .Include(c => c.SubCategories)
            .Where(c => c.Id == categoryId);
    }
}
