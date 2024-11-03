using Budget.Application.Accounts.Commands.Create;

namespace Budget.Tests.Utils.Accounts.Commands;

public static class DeleteAccountCommandMockHelper
{
    // TODO: Accept the arguments
    public static DeleteAccountCommandHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var account = EntityMockHelper.SetupAccount(currency);

        var handler = new DeleteAccountCommandHandler(
            RepositoryMockHelper.SetupAccountRepository(account));

        return handler;
    }

    public static DeleteAccountCommand SetupCommand(
        Guid? id = null,
        string? userId = null)
    {
        var command = new DeleteAccountCommand(
            id ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id
        );

        return command;
    }
}
