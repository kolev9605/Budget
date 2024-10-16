using Budget.Domain.Entities;
using System;
using Budget.Common;

namespace Budget.Domain.Models.Records;

public class RecordsExportModel
{
    public string? Note { get; set; }

    public string? FromAccount { get; set; }

    public string Account { get; set; } = null!;

    public RecordType RecordType { get; set; }

    public string PaymentType { get; set; } = null!;

    public string Category { get; set; } = null!;

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset RecordDate { get; set; }

    public decimal Amount { get; set; }

    public static RecordsExportModel FromRecord(Record record)
    {
        return new RecordsExportModel
        {
            Note = record.Note,
            Account = record.Account.Name,
            FromAccount = record.FromAccount?.Name,
            RecordType = record.RecordType,
            PaymentType = record.PaymentType.Name,
            Category = record.Category.Name,
            DateCreated = record.DateCreated,
            RecordDate = record.RecordDate,
            Amount = record.Amount,
        };
    }

    public static Record ToRecord(RecordsExportModel record)
    {
        return new Record
        {
            Note = record.Note,
            RecordType = record.RecordType,
            DateCreated = record.DateCreated,
            RecordDate = record.RecordDate,
            Amount = record.Amount,
        };
    }
}
