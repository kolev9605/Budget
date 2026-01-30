using Bogus;
using Budget.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class RecordFaker : Faker<Record>
{
    public RecordFaker(Guid accountId, Guid categoryId, Guid? fromAccountId = null)
    {
        RuleFor(r => r.Id, f => Guid.NewGuid());
        RuleFor(r => r.Note, f => f.Lorem.Sentence(5));
        RuleFor(r => r.RecordDate, f => DateTimeOffset.UtcNow.AddDays(-f.Random.Int(0, 365)));
        RuleFor(r => r.AccountId, accountId);
        RuleFor(r => r.CategoryId, categoryId);
        RuleFor(r => r.FromAccountId, fromAccountId);
        RuleFor(r => r.RecordType, f => fromAccountId.HasValue
            ? RecordType.Transfer
            : f.PickRandom(RecordType.Income, RecordType.Expense));
        RuleFor(r => r.Amount, (f, r) =>
        {
            var baseAmount = f.Finance.Amount(10, 5000);
            return r.RecordType == RecordType.Expense ? -Math.Abs(baseAmount) : Math.Abs(baseAmount);
        });
        RuleFor(r => r.CreatedOn, f => DateTimeOffset.UtcNow.AddDays(-f.Random.Int(0, 365)));
        RuleFor(r => r.UpdatedOn, (f, r) => r.CreatedOn.AddDays(f.Random.Int(0, 365)) <= DateTimeOffset.UtcNow
            ? r.CreatedOn.AddDays(f.Random.Int(0, 365))
            : DateTimeOffset.UtcNow);
    }
}
