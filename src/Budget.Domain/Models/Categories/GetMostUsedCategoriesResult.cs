namespace Budget.Domain.Models.Categories;

public record GetMostUsedCategoriesResult(
    Guid Id,
    string Name,
    int Count);
