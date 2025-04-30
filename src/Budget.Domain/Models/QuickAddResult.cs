using Budget.Domain.Entities;

namespace Budget.Domain.Models;

public class QuickAddResult
{
    public string? CategoryName { get; set; }

    public string? AccountName { get; set; }

    public string? FromAccountName { get; set; }

    public decimal Amount { get; set; }

    public DateTime RecordDate { get; set; }

    public string? Note { get; set; }

    public RecordType RecordType { get; set; }
}
