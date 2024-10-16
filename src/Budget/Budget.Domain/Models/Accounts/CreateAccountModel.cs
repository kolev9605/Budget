namespace Budget.Domain.Models.Accounts;

public class CreateAccountModel
{
    public string Name { get; set; } = null!;

    public Guid CurrencyId { get; set; }

    public decimal InitialBalance { get; set; }
}
