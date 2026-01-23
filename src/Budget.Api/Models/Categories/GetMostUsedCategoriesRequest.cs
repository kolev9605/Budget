namespace Budget.Api.Models.Categories;

public record GetMostUsedCategoriesRequest(
    int Count = 4);
