using Budget.Api.Endpoints.Records;
using Budget.Domain.Entities;
using Record = Budget.Domain.Entities.Record;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetAllRecordsEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllRecordsEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<(Account account1, Account account2, Category category1, Category category2)> CreateAccountsAndCategoriesAsync(string userId)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category1 = new CategoryFaker().Generate();
        var category2 = new CategoryFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync();

        var account1 = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        var account2 = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.AddRange(account1, account2);
        await _dbContext.SaveChangesAsync();

        return (account1, account2, category1, category2);
    }

    [Fact]
    public async Task Handle_WithNoRecords_ShouldReturnEmptyPagedList()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetAllRecordsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllRecordsEndpoint.Query(null, null, null, null, null, 1, 10, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Empty(result.Value.Items);
        Assert.Equal(1, result.Value.PageNumber);
        Assert.Equal(0, result.Value.TotalPages);
        Assert.False(result.Value.HasNextPage);
        Assert.False(result.Value.HasPreviousPage);
    }

    [Fact]
    public async Task Handle_WithFilters_ShouldReturnFilteredRecords()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, account2, category1, category2) = await CreateAccountsAndCategoriesAsync(userId);

        var record1 = new RecordFaker(account1.Id, category1.Id)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .Generate();
        var record2 = new RecordFaker(account1.Id, category2.Id)
            .RuleFor(r => r.RecordType, RecordType.Income)
            .Generate();
        var record3 = new RecordFaker(account2.Id, category1.Id)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .Generate();

        _dbContext.Records.AddRange(record1, record2, record3);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllRecordsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllRecordsEndpoint.Query(account1.Id, RecordType.Expense, category1.Id, null, null, 1, 10, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var items = result.Value.Items.ToList();
        Assert.Single(items);
        Assert.Equal(record1.Id, items[0].Id);
    }

    [Fact]
    public async Task Handle_WithDateRange_ShouldReturnRecordsInRange()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category1, _) = await CreateAccountsAndCategoriesAsync(userId);

        var inRangeRecord = new RecordFaker(account1.Id, category1.Id)
            .RuleFor(r => r.RecordDate, DateTimeOffset.UtcNow.AddDays(-2))
            .Generate();
        var outOfRangeRecord = new RecordFaker(account1.Id, category1.Id)
            .RuleFor(r => r.RecordDate, DateTimeOffset.UtcNow.AddDays(-30))
            .Generate();

        _dbContext.Records.AddRange(inRangeRecord, outOfRangeRecord);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllRecordsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllRecordsEndpoint.Query(
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow,
            1,
            10,
            userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var items = result.Value.Items.ToList();
        Assert.Single(items);
        Assert.Equal(inRangeRecord.Id, items[0].Id);
    }

    [Fact]
    public async Task Handle_WithPagination_ShouldReturnPagedResults()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category1, _) = await CreateAccountsAndCategoriesAsync(userId);

        var records = new List<Record>
        {
            new RecordFaker(account1.Id, category1.Id).Generate(),
            new RecordFaker(account1.Id, category1.Id).Generate(),
            new RecordFaker(account1.Id, category1.Id).Generate()
        };

        _dbContext.Records.AddRange(records);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllRecordsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllRecordsEndpoint.Query(null, null, null, null, null, 1, 2, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(2, result.Value.Items.Count());
        Assert.Equal(2, result.Value.TotalPages);
        Assert.True(result.Value.HasNextPage);
        Assert.False(result.Value.HasPreviousPage);
    }
}
