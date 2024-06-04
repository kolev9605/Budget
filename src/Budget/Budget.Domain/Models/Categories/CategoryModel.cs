using Budget.Domain.Entities;

namespace Budget.Domain.Models.Categories;

public record CategoryModel(
    int Id,
    string Name,
    CategoryType CategoryType,
    int? ParentCategoryId,
    bool IsInitial,
    IEnumerable<CategoryModel> SubCategories);
