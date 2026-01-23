using Budget.Application.Accounts.Commands;

namespace Budget.Tests.Utils.Accounts.Commands;

public static class UpdateAccountCommandMockHelper
{
    // TODO: Accept the arguments
    public static UpdateAccountCommandHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);
        var paymentType = EntityMockHelper.SetupPaymentType();

        var handler = new UpdateAccountCommandHandler(
            RepositoryMockHelper.SetupCurrencyRepository(currency),
            RepositoryMockHelper.SetupAccountRepository(account),
            RepositoryMockHelper.SetupPaymentTypeRepository(paymentType));

        return handler;
    }

    public static UpdateAccountCommand SetupCommand(
        Guid? id = null,
        string? name = null,
        Guid? currencyId = null,
        Guid? paymentTypeId = null,
        decimal? initialBalance = null,
        string? userId = null)
    {
        var command = new UpdateAccountCommand(
            id ?? DefaultValueConstants.Common.Id,
            name ?? DefaultValueConstants.Account.DefaultName,
            currencyId ?? DefaultValueConstants.Common.Id,
            paymentTypeId ?? DefaultValueConstants.Common.Id,
            initialBalance ?? DefaultValueConstants.Account.DefaultInitialBalance,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
