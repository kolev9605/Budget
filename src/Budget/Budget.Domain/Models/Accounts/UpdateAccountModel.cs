namespace Budget.Domain.Models.Accounts;

public class UpdateAccountModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal InitialBalance { get; set; }

    public int CurrencyId { get; set; }
}
