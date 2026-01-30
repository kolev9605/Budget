using Budget.Api.Endpoints.Accounts;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Accounts;

public class GetAccountByIdEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAccountByIdEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<(Account account, Category category)> CreateAccountWithDependenciesAsync(string userId, decimal initialBalance)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category = new CategoryFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.InitialBalance, initialBalance)
            .Generate();

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return (account, category);
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldReturnAccountWithBalance()
    {
        // Arrange
        var initialBalance = 1000m;
        var recordAmount1 = -100m; // Expense
        var recordAmount2 = 50m;   // Income
        var userId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountWithDependenciesAsync(userId, initialBalance);

        var expenseRecord = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .RuleFor(r => r.Amount, recordAmount1)
            .Generate();

        var incomeRecord = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordType, RecordType.Income)
            .RuleFor(r => r.Amount, recordAmount2)
            .Generate();

        _dbContext.Records.AddRange(expenseRecord, incomeRecord);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAccountByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetAccountByIdEndpoint.Query(account.Id, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var response = result.Value;

        var expectedBalance = initialBalance + recordAmount1 + recordAmount2;
        Assert.Equal(account.Id, response.Id);
        Assert.Equal(account.Name, response.Name);
        Assert.Equal(initialBalance, response.InitialBalance);
        Assert.Equal(expectedBalance, response.Balance);
        Assert.Equal(account.CurrencyId, response.Currency.Id);
        Assert.Equal(account.PaymentTypeId, response.PaymentType.Id);
        Assert.Equal(account.IsActive, response.IsActive);
    }

    [Fact]
    public async Task Handle_WithNonexistentAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetAccountByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetAccountByIdEndpoint.Query(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task Handle_WithDifferentUser_ShouldReturnError()
    {
        // Arrange
        var initialBalance = 500m;
        var ownerUserId = await CreateTestUserAsync();
        var otherUserId = await CreateTestUserAsync();
        var (account, _) = await CreateAccountWithDependenciesAsync(ownerUserId, initialBalance);

        var handler = new GetAccountByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetAccountByIdEndpoint.Query(account.Id, otherUserId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }
}
