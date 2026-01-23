using Budget.Application.Records.Commands;

namespace Budget.Tests.Utils.Records.Commands;

public static class CreateRecordCommandMockHelper
{
    public static CreateRecordCommandHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var currency = EntityMockHelper.SetupCurrency();
        var category = EntityMockHelper.SetupCategory(user);
        var account = EntityMockHelper.SetupAccount(currency);
        var record = EntityMockHelper.SetupRecord(account, category);

        var handler = new CreateRecordCommandHandler(
            ServiceMockHelper.SetupDateTimeProvider(),
            RepositoryMockHelper.SetupRecordRepository(record),
            RepositoryMockHelper.SetupAccountRepository(account),
            ServiceMockHelper.SetupUserService(),
            RepositoryMockHelper.SetupCategoryRepository(category));

        return handler;
    }

    public static CreateRecordCommand SetupCommand(
        Guid? accountId = null,
        Guid? fromAccountId = null,
        Guid? categoryId = null,
        string? userId = null)
    {
        var command = new CreateRecordCommand(
            DefaultValueConstants.Common.Id.GenerateRecordNote(),
            DefaultValueConstants.Record.Amount,
            accountId ?? DefaultValueConstants.Common.Id,
            categoryId ?? DefaultValueConstants.Common.Id,
            DefaultValueConstants.Record.Type,
            DefaultValueConstants.Record.CreationDate,
            fromAccountId,
            userId ?? DefaultValueConstants.User.Id);

        return command;
    }
}
