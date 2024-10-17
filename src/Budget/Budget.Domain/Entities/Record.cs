using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class Record : BaseEntity, ICreatable, IUpdatable
{
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

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
