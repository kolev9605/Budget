using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class Record : BaseEntity, ICreatable, IUpdatable
{
    public Record() { }

    public Record(
        string? note,
        DateTimeOffset recordDate,
        decimal amount,
        Guid accountId,
        Guid? fromAccountId,
        Guid paymentTypeId,
        Guid categoryId,
        RecordType recordType,
        DateTimeOffset createdOn,
        bool isNegativeTransferRecord = false)
    {
        Note = note;
        RecordDate = recordDate;
        AccountId = accountId;
        FromAccountId = fromAccountId;
        PaymentTypeId = paymentTypeId;
        CategoryId = categoryId;
        RecordType = recordType;
        CreatedOn = createdOn;
        Amount = GetAmountByRecordType(amount, isNegativeTransferRecord);
    }

    public string? Note { get; set; }

    public DateTimeOffset RecordDate { get; set; }

    public decimal Amount { get; set; }

    public Guid AccountId { get; set; }

    public Account Account { get; set; } = null!;

    public Guid? FromAccountId { get; set; }

    public Account? FromAccount { get; set; }

    public Guid PaymentTypeId { get; set; }

    public PaymentType PaymentType { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public RecordType RecordType { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset UpdatedOn { get; set; }

    public decimal GetAmountByRecordType(decimal amount, bool isNegativeTransferRecord = false)
    {
        if (RecordType == RecordType.Expense || (RecordType == RecordType.Transfer && isNegativeTransferRecord))
        {
            return -Math.Abs(amount);
        }

        return Math.Abs(amount);
    }

    public Record CreateNegativeTransferRecord()
    {
        var negativeTransferRecord = new Record(
            Note,
            RecordDate,
            Amount,
            // TODO: Maybe validate here?
            FromAccountId!.Value,
            AccountId,
            PaymentTypeId,
            CategoryId,
            RecordType,
            CreatedOn,
            true);

        return negativeTransferRecord;
    }

    public void Update(
        string note,
        DateTimeOffset recordDate,
        decimal amount,
        Guid accountId,
        Guid? fromAccountId,
        Guid paymentTypeId,
        Guid categoryId,
        RecordType recordType,
        DateTimeOffset utcNow,
        bool isNegativeTransferRecord = false)
    {
        Note = note;
        RecordDate = recordDate;
        AccountId = accountId;
        FromAccountId = fromAccountId;
        PaymentTypeId = paymentTypeId;
        CategoryId = categoryId;
        RecordType = recordType;
        UpdatedOn = utcNow;
        Amount = GetAmountByRecordType(amount, isNegativeTransferRecord);
    }

}
