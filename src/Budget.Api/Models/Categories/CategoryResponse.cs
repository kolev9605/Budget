using Budget.Domain.Entities;

namespace Budget.Api.Models.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    // IEnumerable<SubCategoryResponse> SubCategories,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);

public record SubCategoryResponse(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
