using Budget.Application.Accounts.Commands;

namespace Budget.Tests.Utils.Accounts.Commands;

public static class CreateAccountCommandMockHelper
{
    // TODO: Accept the arguments
    public static CreateAccountCommandHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);

        var handler = new CreateAccountCommandHandler(
            RepositoryMockHelper.SetupCurrencyRepository(currency),
            RepositoryMockHelper.SetupAccountRepository(account));

        return handler;
    }

    public static CreateAccountCommand SetupCommand(
        string? name = null,
        Guid? currencyId = null,
        decimal? initialBalance = null,
        string? userId = null)
    {
        var command = new CreateAccountCommand(
            name ?? DefaultValueConstants.Account.DefaultName,
            currencyId ?? DefaultValueConstants.Common.Id,
            initialBalance ?? DefaultValueConstants.Account.DefaultInitialBalance,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
