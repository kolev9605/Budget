using Budget.Application.Records.Commands;

namespace Budget.Tests.Utils.Records.Commands;

public static class UpdateRecordCommandMockHelper
{
    // TODO: Accept the arguments
    public static UpdateRecordCommandHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var currency = EntityMockHelper.SetupCurrency();
        var category = EntityMockHelper.SetupCategory(user);
        var account = EntityMockHelper.SetupAccount(currency);
        var record = EntityMockHelper.SetupRecord(account, category);

        var handler = new UpdateRecordCommandHandler(
            ServiceMockHelper.SetupDateTimeProvider(),
            RepositoryMockHelper.SetupRecordRepository(record),
            RepositoryMockHelper.SetupAccountRepository(account),
            ServiceMockHelper.SetupUserService(),
            RepositoryMockHelper.SetupCategoryRepository(category)
        );

        return handler;
    }

    public static UpdateRecordCommand SetupCommand(
        Guid? recordId = null,
        Guid? accountId = null,
        Guid? fromAccountId = null,
        Guid? categoryId = null,
        string? userId = null)
    {
        var command = new UpdateRecordCommand(
            recordId ?? DefaultValueConstants.Common.Id,
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
