using Budget.Api.Domain.Entities.Base;

namespace Budget.Api.Domain.Entities;

public class PaymentType : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
