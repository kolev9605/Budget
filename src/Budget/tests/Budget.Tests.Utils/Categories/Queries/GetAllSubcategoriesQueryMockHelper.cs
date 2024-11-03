using Budget.Application.Categories.Queries.GetById;

namespace Budget.Tests.Utils.Categories.Queries;

public static class GetAllSubcategoriesQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllSubcategoriesQueryHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var category = EntityMockHelper.SetupCategory(user);

        var handler = new GetAllSubcategoriesQueryHandler(
            RepositoryMockHelper.SetupCategoryRepository(category));

        return handler;
    }

    public static GetAllSubcategoriesQuery SetupQuery(
        Guid? parentCategoryId = null,
        string? userId = null
    )
    {
        var query = new GetAllSubcategoriesQuery(
            parentCategoryId ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id
        );

        return query;
    }
}
