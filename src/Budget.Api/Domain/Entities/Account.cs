using Budget.Api.Domain.Entities.Base;

namespace Budget.Api.Domain.Entities;

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

    public Guid PaymentTypeId { get; set; }

    public PaymentType PaymentType { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset UpdatedOn { get; set; }

    public bool IsActive { get; set; } = true;

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
