using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class PaymentType : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
