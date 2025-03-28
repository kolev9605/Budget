using Budget.Application.Accounts.Queries.GetById;

namespace Budget.Tests.Utils.Accounts.Queries;

public static class GetAccountByIdQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAccountByIdQueryHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);

        var handler = new GetAccountByIdQueryHandler(
            RepositoryMockHelper.SetupAccountRepository(account));

        return handler;
    }

    public static GetAccountByIdQuery SetupQuery(
        Guid? id = null,
        string? userId = null
    )
    {
        var command = new GetAccountByIdQuery(
            id ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
