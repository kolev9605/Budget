using Budget.Application.Categories.Queries.GetById;

namespace Budget.Tests.Utils.Categories.Queries;

public static class GetAllCategoriesQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllCategoriesQueryHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var category = EntityMockHelper.SetupCategory(user);

        var handler = new GetAllCategoriesQueryHandler(
            RepositoryMockHelper.SetupCategoryRepository(category));

        return handler;
    }

    public static GetAllCategoriesQuery SetupQuery(
        bool? primaryOnly = null,
        string? userId = null
    )
    {
        var query = new GetAllCategoriesQuery(
            primaryOnly ?? false,
            userId ?? DefaultValueConstants.User.Id
        );

        return query;
    }
}
