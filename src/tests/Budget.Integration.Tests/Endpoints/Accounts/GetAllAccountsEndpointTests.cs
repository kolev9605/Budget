using Budget.Api.Endpoints.Accounts;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Accounts;

public class GetAllAccountsEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllAccountsEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<(List<Account> accounts, Currency currency, PaymentType paymentType)> CreateAccountsWithDependenciesAsync(
        string userId, int accountCount, int activeCount)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var accounts = new List<Account>();
        for (int i = 0; i < accountCount; i++)
        {
            var account = new AccountFaker(userId, currency.Id, paymentType.Id)
                .RuleFor(a => a.Name, $"Account {userId} {i + 1}")
                .RuleFor(a => a.IsActive, i < activeCount)
                .Generate();

            accounts.Add(account);
        }

        _dbContext.Accounts.AddRange(accounts);
        await _dbContext.SaveChangesAsync();

        return (accounts, currency, paymentType);
    }

    [Fact]
    public async Task Handle_WithNoAccounts_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Empty(accounts);
    }

    [Fact]
    public async Task Handle_WithMultipleAccounts_ShouldReturnAllActiveAccounts()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (createdAccounts, currency, paymentType) = await CreateAccountsWithDependenciesAsync(
            userId, accountCount: 3, activeCount: 3);

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Equal(3, accounts.Count);

        // Verify all accounts are present
        foreach (var expectedAccount in createdAccounts)
        {
            var responseAccount = accounts.FirstOrDefault(a => a.Id == expectedAccount.Id);
            Assert.NotNull(responseAccount);
            Assert.Equal(expectedAccount.Name, responseAccount.Name);
            Assert.Equal(expectedAccount.InitialBalance, responseAccount.InitialBalance);
            Assert.Equal(expectedAccount.IsActive, responseAccount.IsActive);
            Assert.Equal(currency.Id, responseAccount.Currency.Id);
            Assert.Equal(currency.Name, responseAccount.Currency.Name);
            Assert.Equal(paymentType.Id, responseAccount.PaymentType.Id);
            Assert.Equal(paymentType.Name, responseAccount.PaymentType.Name);
        }
    }

    [Fact]
    public async Task Handle_WithMixedActiveInactive_IncludeHiddenFalse_ShouldReturnOnlyActiveAccounts()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (createdAccounts, _, _) = await CreateAccountsWithDependenciesAsync(
            userId, accountCount: 4, activeCount: 2);

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Equal(2, accounts.Count);

        // Verify all returned accounts are active
        foreach (var account in accounts)
        {
            Assert.True(account.IsActive);
        }

        // Verify inactive accounts are not included
        var inactiveIds = createdAccounts.Where(a => !a.IsActive).Select(a => a.Id).ToList();
        foreach (var inactiveId in inactiveIds)
        {
            Assert.DoesNotContain(accounts, a => a.Id == inactiveId);
        }
    }

    [Fact]
    public async Task Handle_WithMixedActiveInactive_IncludeHiddenTrue_ShouldReturnAllAccounts()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (createdAccounts, _, _) = await CreateAccountsWithDependenciesAsync(
            userId, accountCount: 4, activeCount: 2);

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, true);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Equal(4, accounts.Count);

        // Verify both active and inactive are included
        var activeCount = accounts.Count(a => a.IsActive);
        var inactiveCount = accounts.Count(a => !a.IsActive);
        Assert.Equal(2, activeCount);
        Assert.Equal(2, inactiveCount);
    }

    [Fact]
    public async Task Handle_WithRecords_ShouldCalculateCorrectBalance()
    {
        // Arrange
        var initialBalance = 1000m;
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category = new CategoryFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.InitialBalance, initialBalance)
            .RuleFor(a => a.IsActive, true)
            .Generate();

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        // Create records: expense -200, income +150
        var expenseRecord = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .RuleFor(r => r.Amount, -200m)
            .Generate();

        var incomeRecord = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordType, RecordType.Income)
            .RuleFor(r => r.Amount, 150m)
            .Generate();

        _dbContext.Records.AddRange(expenseRecord, incomeRecord);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Single(accounts);

        var responseAccount = accounts[0];
        var expectedBalance = initialBalance + (-200m) + 150m;
        Assert.Equal(expectedBalance, responseAccount.Balance);
    }

    [Fact]
    public async Task Handle_WithMultipleUsers_ShouldOnlyReturnUserAccounts()
    {
        // Arrange
        var userId1 = await CreateTestUserAsync();
        var userId2 = await CreateTestUserAsync();

        var (user1Accounts, currency, paymentType) = await CreateAccountsWithDependenciesAsync(
            userId1, accountCount: 2, activeCount: 2);

        var (user2Accounts, user2Currency, user2PaymentType) = await CreateAccountsWithDependenciesAsync(
            userId2, accountCount: 3, activeCount: 2);

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);

        // Act - Query for first user
        var result1 = await handler.Handle(new GetAllAccountsEndpoint.Query(userId1, false), CancellationToken.None);

        // Act - Query for second user
        var result2 = await handler.Handle(new GetAllAccountsEndpoint.Query(userId2, false), CancellationToken.None);

        // Assert
        Assert.False(result1.IsError);
        Assert.False(result2.IsError);

        var user1Results = result1.Value.ToList();
        var user2Results = result2.Value.ToList();

        Assert.Equal(2, user1Results.Count);
        Assert.Equal(2, user2Results.Count);

        // Verify isolation
        var user1Ids = user1Results.Select(a => a.Id).ToList();
        var user2Ids = user2Results.Select(a => a.Id).ToList();

        // User 1 should not see user 2's accounts
        foreach (var user2AccountId in user2Ids)
        {
            Assert.DoesNotContain(user2AccountId, user1Ids);
        }

        // User 2 should not see user 1's accounts
        foreach (var user1AccountId in user1Ids)
        {
            Assert.DoesNotContain(user1AccountId, user2Ids);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectResponseStructure()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.Name, "Test Account")
            .RuleFor(a => a.InitialBalance, 5000m)
            .Generate();

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);
        var query = new GetAllAccountsEndpoint.Query(userId, false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var accounts = result.Value.ToList();
        Assert.Single(accounts);

        var responseAccount = accounts[0];

        // Verify all response fields are populated
        Assert.NotEqual(Guid.Empty, responseAccount.Id);
        Assert.Equal("Test Account", responseAccount.Name);
        Assert.Equal(5000m, responseAccount.InitialBalance);
        Assert.Equal(5000m, responseAccount.Balance); // No records, so balance = initial balance
        Assert.NotNull(responseAccount.Currency);
        Assert.NotEqual(Guid.Empty, responseAccount.Currency.Id);
        Assert.Equal(currency.Name, responseAccount.Currency.Name);
        Assert.NotNull(responseAccount.PaymentType);
        Assert.NotEqual(Guid.Empty, responseAccount.PaymentType.Id);
        Assert.Equal(paymentType.Name, responseAccount.PaymentType.Name);
        Assert.True(responseAccount.IsActive);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccessResult()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetAllAccountsEndpoint.QueryHandler(_dbContext);

        // Act - Test with empty
        var emptyResult = await handler.Handle(new GetAllAccountsEndpoint.Query(userId, false), CancellationToken.None);

        // Assert - Empty list should still be success
        Assert.False(emptyResult.IsError);
        Assert.Empty(emptyResult.Value);

        // Arrange - Create an account
        var (_, currency, paymentType) = await CreateAccountsWithDependenciesAsync(userId, 1, 1);

        // Act - Test with data
        var filledResult = await handler.Handle(new GetAllAccountsEndpoint.Query(userId, false), CancellationToken.None);

        // Assert - Non-empty should also be success
        Assert.False(filledResult.IsError);
        Assert.Single(filledResult.Value);
    }
}
