using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Records;

namespace Budget.Tests.Utils;

public static class ModelMockHelper
{
    public static CreateRecordModel CreateRecordModel(Guid? accountId = null)
    {
        var model = new CreateRecordModel()
        {
            AccountId = accountId ?? DefaultValueConstants.Common.Id,
            Amount = DefaultValueConstants.Record.Amount,
            CategoryId = DefaultValueConstants.Common.Id,
            Note = DefaultValueConstants.Common.Id.GenerateRecordNote(),
            PaymentTypeId = DefaultValueConstants.Common.Id,
            RecordType = DefaultValueConstants.Record.Type,
        };

        return model;
    }

    public static CreateAccountModel CreateAccountModel(string name = DefaultValueConstants.Account.DefaultName)
    {
        var createAccountRequest = new CreateAccountModel()
        {
            CurrencyId = DefaultValueConstants.Common.Id,
            InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
            Name = name
        };

        return createAccountRequest;
    }
}
