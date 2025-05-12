using Budget.Application.Categories.Queries;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Records.Create;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(BudgetDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<CategoryModel>> GetAllAsync(string userId)
    {
        var categories = await GetUserCategories(userId)
            .OrderBy(c => c.ParentCategoryId ?? c.Id)
            .ThenBy(c => c.Id)
            .ProjectToType<CategoryModel>()
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId)
    {
        var categories = await GetUserCategories(userId)
            .Where(c => c.ParentCategoryId == null)
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

    public async Task<CategoryForRecordCreationModel?> GetForRecordCreationAsync(Guid id)
    {
        return await GetAll()
            .Where(c => c.Id == id)
            .ProjectToType<CategoryForRecordCreationModel>()
            .FirstOrDefaultAsync();
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

    public async Task<IEnumerable<GetMostUsedCategoriesResult>> GetMostUsedCategoriesAsync(string userId, int count, int recordsCount = 1000)
    {
        // Get last 1000 records for the user, including Category
        var lastRecords = await _budgetDbContext.Records
            .Where(r => r.Account.UserId == userId)
            .Where(r => r.Category.CategoryType != CategoryType.Transfer)
            .OrderByDescending(r => r.RecordDate)
            .Take(recordsCount)
            .Include(r => r.Category)
            .ToListAsync();

        // Group and project in-memory
        return lastRecords
            .Where(r => r.Category != null)
            .GroupBy(r => new { r.Category.Id, r.Category.Name })
            .Select(g => new GetMostUsedCategoriesResult(g.Key.Id, g.Key.Name, g.Count()))
            .OrderByDescending(g => g.Count)
            .Take(count)
            .ToList();
    }
}
