using Budget.Application.Accounts.Commands.Create;

namespace Budget.Tests.Utils.Accounts.Commands;

public static class UpdateAccountCommandMockHelper
{
    // TODO: Accept the arguments
    public static UpdateAccountCommandHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);

        var handler = new UpdateAccountCommandHandler(
            RepositoryMockHelper.SetupCurrencyRepository(currency),
            RepositoryMockHelper.SetupAccountRepository(account));

        return handler;
    }

    public static UpdateAccountCommand SetupCommand(
        Guid? id = null,
        string? name = null,
        Guid? currencyId = null,
        decimal? initialBalance = null,
        string? userId = null)
    {
        var command = new UpdateAccountCommand(
            id ?? DefaultValueConstants.Common.Id,
            name ?? DefaultValueConstants.Account.DefaultName,
            currencyId ?? DefaultValueConstants.Common.Id,
            initialBalance ?? DefaultValueConstants.Account.DefaultInitialBalance,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
