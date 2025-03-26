using Budget.Application.Categories.Queries.GetById;

namespace Budget.Tests.Utils.Categories.Queries;

public static class GetCategoryByIdQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetCategoryByIdQueryHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var category = EntityMockHelper.SetupCategory(user);

        var handler = new GetCategoryByIdQueryHandler(
            RepositoryMockHelper.SetupCategoryRepository(category));

        return handler;
    }

    public static GetCategoryByIdQuery SetupQuery(
        Guid? categoryId = null,
        string? userId = null
    )
    {
        var query = new GetCategoryByIdQuery(
            categoryId ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id
        );

        return query;
    }
}
