namespace Budget.Domain.Models.Accounts;

public class CreateAccountModel
{
    public string Name { get; set; } = null!;

    public int CurrencyId { get; set; }

    public decimal InitialBalance { get; set; }
}
