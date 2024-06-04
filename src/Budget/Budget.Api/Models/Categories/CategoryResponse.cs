using Budget.Domain.Entities;

namespace Budget.Api.Models.Categories;

public record CategoryResponse(
    int Id,
    string Name,
    CategoryType CategoryType,
    int? ParentCategoryId,
    bool IsInitial,
    IEnumerable<CategoryResponse> SubCategories);
