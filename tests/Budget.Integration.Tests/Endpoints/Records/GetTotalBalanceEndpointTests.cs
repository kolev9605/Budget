using Budget.Api.Endpoints.Records;
using Budget.Api.Domain.Entities;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Budget.Api.Infrastructure.Persistence;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetTotalBalanceEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetTotalBalanceEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
    }

    private async Task<string> CreateTestUserAsync()
    {
        var user = new ApplicationUserFaker().Generate();
        var result = await _userManager.CreateAsync(user, "Password1!");

        await _dbContext.SaveChangesAsync();
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.Id;
    }

    [Fact]
    public async Task Handle_ShouldReturnTotalBalance()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category = new CategoryFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var account1 = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.InitialBalance, 100m)
            .Generate();
        var account2 = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.InitialBalance, 50m)
            .Generate();

        _dbContext.Accounts.AddRange(account1, account2);
        await _dbContext.SaveChangesAsync();

        var record1 = new RecordFaker(account1.Id, category.Id)
            .RuleFor(r => r.Amount, -20m)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .Generate();
        var record2 = new RecordFaker(account2.Id, category.Id)
            .RuleFor(r => r.Amount, 10m)
            .RuleFor(r => r.RecordType, RecordType.Income)
            .Generate();

        _dbContext.Records.AddRange(record1, record2);
        await _dbContext.SaveChangesAsync();

        var handler = new GetTotalBalanceEndpoint.QueryHandler(_dbContext);
        var query = new GetTotalBalanceEndpoint.Query(userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(140m, result);
    }

    [Fact]
    public async Task Handle_WithNoAccounts_ShouldReturnZero()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetTotalBalanceEndpoint.QueryHandler(_dbContext);
        var query = new GetTotalBalanceEndpoint.Query(userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(0m, result);
    }
}
