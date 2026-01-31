using Budget.Api.Endpoints.Records;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetRecordsDateRangeEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetRecordsDateRangeEndpointTests(DatabaseFixtureApp fixture)
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
    public async Task Handle_WithNoRecords_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetRecordsDateRangeEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordsDateRangeEndpoint.Query(userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NoRecords.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithRecords_ShouldReturnMinAndMaxDates()
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

        var minDate = DateTimeOffset.UtcNow.AddDays(-10);
        var maxDate = DateTimeOffset.UtcNow.AddDays(-2);

        var record1 = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordDate, minDate)
            .Generate();
        var record2 = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordDate, maxDate)
            .Generate();

        _dbContext.Records.AddRange(record1, record2);
        await _dbContext.SaveChangesAsync();

        var handler = new GetRecordsDateRangeEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordsDateRangeEndpoint.Query(userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var minDiff = (result.Value.MinDate!.Value - minDate).Duration();
        var maxDiff = (result.Value.MaxDate!.Value - maxDate).Duration();
        Assert.True(minDiff < TimeSpan.FromMilliseconds(1));
        Assert.True(maxDiff < TimeSpan.FromMilliseconds(1));
    }
}
