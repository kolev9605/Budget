using Budget.Domain.Models.Records;

namespace Budget.Tests.Utils;

public static class ModelMockHelper
{
    public static CreateRecordModel CreateRecordModel(int? accountId = null)
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
}
