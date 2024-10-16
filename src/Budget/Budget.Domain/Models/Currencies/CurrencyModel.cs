namespace Budget.Domain.Models.Currencies;

public class CurrencyModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public string SpecialName { get; set; } = null!;
}
