using Budget.Domain.Entities;

namespace Budget.Api.Models.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    IEnumerable<SubCategoryResponse> SubCategories,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public record SubCategoryResponse(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
