namespace Budget.Domain.Models.Accounts;

public class UpdateAccountModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal InitialBalance { get; set; }

    public Guid CurrencyId { get; set; }
}
