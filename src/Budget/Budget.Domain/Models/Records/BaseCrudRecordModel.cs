using Budget.Domain.Entities;
using System;

namespace Budget.Domain.Models.Records;

public abstract class BaseCrudRecordModel
{
    public string Note { get; set; } = null!;

    public decimal Amount { get; set; }

    public Guid AccountId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid PaymentTypeId { get; set; }

    public RecordType RecordType { get; set; }

    public DateTime RecordDate { get; set; }

    public Guid? FromAccountId { get; set; }
}
