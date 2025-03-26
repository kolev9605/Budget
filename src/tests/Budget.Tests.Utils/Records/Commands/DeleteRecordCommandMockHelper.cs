using Budget.Application.Records.Commands;

namespace Budget.Tests.Utils.Records.Commands;

public static class DeleteRecordCommandMockHelper
{
    // TODO: Accept the arguments
    public static DeleteRecordCommandHandler SetupHandler()
    {
        var user = EntityMockHelper.SetupUser();
        var currency = EntityMockHelper.SetupCurrency();
        var paymentType = EntityMockHelper.SetupPaymentType();
        var category = EntityMockHelper.SetupCategory(user);
        var account = EntityMockHelper.SetupAccount(currency);
        var record = EntityMockHelper.SetupRecord(account, paymentType, category);

        var handler = new DeleteRecordCommandHandler(
            RepositoryMockHelper.SetupRecordRepository(record));

        return handler;
    }

    public static DeleteRecordCommand SetupCommand(
        Guid? recordId = null,
        string? userId = null)
    {
        var command = new DeleteRecordCommand(
            recordId ?? DefaultValueConstants.Common.Id,
            userId ?? DefaultValueConstants.User.Id);

        return command;
    }
}
