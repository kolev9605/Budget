using Budget.Domain.Entities;

namespace Budget.Domain.Models.Categories;

public record CategoryModel(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    IEnumerable<CategoryModel> SubCategories);
