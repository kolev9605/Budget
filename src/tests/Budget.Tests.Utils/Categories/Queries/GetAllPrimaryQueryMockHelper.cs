using Budget.Application.Categories.Queries.GetById;

namespace Budget.Tests.Utils.Categories.Queries;

public static class GetAllPrimaryQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllPrimaryQueryHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var category = EntityMockHelper.SetupCategory(user);

        var handler = new GetAllPrimaryQueryHandler(
            RepositoryMockHelper.SetupCategoryRepository(category));

        return handler;
    }

    public static GetAllPrimaryQuery SetupQuery(
        string? userId = null
    )
    {
        var query = new GetAllPrimaryQuery(
            userId ?? DefaultValueConstants.User.Id
        );

        return query;
    }
}
