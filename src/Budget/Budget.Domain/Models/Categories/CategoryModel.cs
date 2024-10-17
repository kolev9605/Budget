using Budget.Domain.Entities;

namespace Budget.Domain.Models.Categories;

public record CategoryModel(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    // TODO: Separate model for the categories
    IEnumerable<CategoryModel> SubCategories,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
