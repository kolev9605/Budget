using Bogus;
using Budget.Api.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        RuleFor(c => c.Id, f => Guid.NewGuid());
        RuleFor(c => c.Name, f => f.PickRandom(new[]
        {
            // Needs
            "Housing", "Utilities", "Groceries", "Transportation", "Healthcare", "Insurance",
            // Wants
            "Entertainment", "Dining Out", "Shopping", "Travel", "Hobbies", "Subscriptions",
            // Income
            "Salary", "Freelance", "Investment Income", "Bonus", "Gift",
            // Transfer
            "Savings Transfer", "Account Transfer", "Investment Transfer"
        }));
        RuleFor(c => c.CategoryType, f => f.PickRandom<CategoryType>());
        RuleFor(c => c.IsInitial, f => f.Random.Bool(0.3f)); // 30% chance of being initial
        RuleFor(c => c.CreatedOn, f => DateTimeOffset.UtcNow.AddDays(-f.Random.Int(0, 365)));
        RuleFor(c => c.UpdatedOn, (f, c) => c.CreatedOn.AddDays(f.Random.Int(0, 365)) <= DateTimeOffset.UtcNow
            ? c.CreatedOn.AddDays(f.Random.Int(0, 365))
            : DateTimeOffset.UtcNow);
        RuleFor(c => c.ParentCategoryId, f => null); // No parent by default
    }
}
