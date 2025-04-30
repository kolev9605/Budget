using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;

namespace Budget.Domain.Models.Records;

public class RecordModel
{
    public Guid Id { get; set; }

    public string Note { get; set; } = null!;

    public AccountModel? FromAccount { get; set; }

    public AccountModel Account { get; set; } = null!;

    public RecordType RecordType { get; set; }

    public CategoryModel Category { get; set; } = null!;

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset RecordDate { get; set; }

    public decimal Amount { get; set; }

    public string ToCsv()
    {
        return $"{Amount},{Account.Name},{FromAccount?.Name ?? ""},{Category.Name},{Note},{RecordDate.ToString("yyyy-MM-dd")},{RecordType}";
    }
}
