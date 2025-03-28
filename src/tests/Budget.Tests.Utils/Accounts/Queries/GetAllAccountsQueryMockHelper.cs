using Budget.Application.Accounts.Queries.GetAll;

namespace Budget.Tests.Utils.Accounts.Queries;

public static class GetAllAccountsQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllAccountsQueryHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);

        var handler = new GetAllAccountsQueryHandler(
            RepositoryMockHelper.SetupAccountRepository(account));

        return handler;
    }

    public static GetAllAccountsQuery SetupQuery(
        string? userId = null
    )
    {
        var command = new GetAllAccountsQuery(
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
