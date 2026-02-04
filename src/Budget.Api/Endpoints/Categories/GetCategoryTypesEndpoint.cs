using Budget.Api.Interfaces;
using Budget.Api.Helpers;
using Budget.Api.Domain.Entities;

namespace Budget.Api.Endpoints.Categories;

public class GetCategoryTypesEndpoint : IEndpoint
{
    public static void Map(WebApplication app)
    {
        app
            .MapGet("/categories/types", () =>
            {
                var categoryTypes = EnumHelpers.GetListFromEnum<CategoryType>();
                return Results.Ok(categoryTypes);
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
