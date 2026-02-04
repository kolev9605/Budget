using Bogus;
using Budget.Api.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class AccountFaker : Faker<Account>
{
    public AccountFaker(string userId, Guid? currencyId = null, Guid? paymentTypeId = null)
    {
        RuleFor(a => a.Id, f => Guid.NewGuid());
        RuleFor(a => a.Name, f => f.Finance.AccountName());
        RuleFor(a => a.UserId, userId);
        RuleFor(a => a.CurrencyId, currencyId ?? Guid.NewGuid());
        RuleFor(a => a.PaymentTypeId, paymentTypeId ?? Guid.NewGuid());
        RuleFor(a => a.InitialBalance, f => f.Finance.Amount(-1000, 10000));
        RuleFor(a => a.IsActive, f => f.Random.Bool(0.9f)); // 90% active
        RuleFor(a => a.CreatedOn, f => DateTimeOffset.UtcNow.AddDays(-f.Random.Int(1, 730)));
        RuleFor(a => a.UpdatedOn, (f, a) => a.CreatedOn.AddDays(f.Random.Int(0, 365)) <= DateTimeOffset.UtcNow
            ? a.CreatedOn.AddDays(f.Random.Int(0, 365))
            : DateTimeOffset.UtcNow);
    }
}
