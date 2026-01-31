using Budget.Api.Endpoints.Records;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetRecordsStatisticsEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetRecordsStatisticsEndpointTests(DatabaseFixtureApp fixture)
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
    public async Task Handle_WithDateRange_ShouldReturnRecordsInRange()
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

        var account = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var inRange = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordDate, DateTimeOffset.UtcNow.AddDays(-3))
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .Generate();
        var outOfRange = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordDate, DateTimeOffset.UtcNow.AddDays(-20))
            .Generate();

        _dbContext.Records.AddRange(inRange, outOfRange);
        await _dbContext.SaveChangesAsync();

        var handler = new GetRecordsStatisticsEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordsStatisticsEndpoint.Query(
            DateTimeOffset.UtcNow.AddDays(-7),
            DateTimeOffset.UtcNow,
            userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var items = result.Value.ToList();
        Assert.Single(items);
        Assert.Equal(inRange.Id, items[0].Id);
    }
}
