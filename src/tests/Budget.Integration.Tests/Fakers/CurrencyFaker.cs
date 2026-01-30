using Bogus;
using Budget.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class CurrencyFaker : Faker<Currency>
{
    public CurrencyFaker()
    {
        RuleFor(c => c.Id, f => Guid.NewGuid());
        RuleFor(c => c.Name, f => f.Finance.Currency().Description);
        RuleFor(c => c.Abbreviation, f => f.Finance.Currency().Code);
    }
}
