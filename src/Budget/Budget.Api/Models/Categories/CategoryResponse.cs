using Budget.Domain.Entities;

namespace Budget.Api.Models.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    IEnumerable<CategoryResponse> SubCategories,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
