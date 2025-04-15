using Budget.Application.Accounts.Commands;

namespace Budget.Tests.Utils.Accounts.Commands;

public static class CreateAccountCommandMockHelper
{
    // TODO: Accept the arguments
    public static CreateAccountCommandHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);
        var paymentType = EntityMockHelper.SetupPaymentType();

        var handler = new CreateAccountCommandHandler(
            RepositoryMockHelper.SetupCurrencyRepository(currency),
            RepositoryMockHelper.SetupAccountRepository(account),
            RepositoryMockHelper.SetupPaymentTypeRepository(paymentType));

        return handler;
    }

    public static CreateAccountCommand SetupCommand(
        string? name = null,
        Guid? currencyId = null,
        Guid? paymentTypeId = null,
        decimal? initialBalance = null,
        string? userId = null)
    {
        var command = new CreateAccountCommand(
            name ?? DefaultValueConstants.Account.DefaultName,
            currencyId ?? DefaultValueConstants.Common.Id,
            initialBalance ?? DefaultValueConstants.Account.DefaultInitialBalance,
            paymentTypeId ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
