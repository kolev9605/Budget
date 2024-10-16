using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class Account : BaseEntity, ICreatable, IUpdatable
{
    public string Name { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    public ICollection<Record> Records { get; set; } = new List<Record>();

    public ICollection<Record> TransferRecords { get; set; } = new List<Record>();

    public Guid CurrencyId { get; set; }

    public Currency Currency { get; set; } = null!;

    public decimal InitialBalance { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
