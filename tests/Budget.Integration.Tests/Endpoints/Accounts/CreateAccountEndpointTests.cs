using Budget.Api.Endpoints.Accounts;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Accounts;

// [Collection("Database collection")]
public class CreateAccountEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateAccountEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
    }

    private async Task<string> CreateTestUserAsync()
    {
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUserFaker().Generate();
        var result = await _userManager.CreateAsync(user, "Password1!");

        await _dbContext.SaveChangesAsync();
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        Assert.NotNull(user.Id);
        Assert.False(string.IsNullOrWhiteSpace(user.SecurityStamp), $"SecurityStamp should not be null or whitespace after user creation");
        Assert.False(string.IsNullOrWhiteSpace(user.ConcurrencyStamp), $"ConcurrencyStamp should not be null or whitespace after user creation");

        return user.Id;
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldCreateAccount()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAccountEndpoint.CommandHandler(_dbContext);
        var command = new CreateAccountEndpoint.Command(
            Name: "My First Account",
            CurrencyId: currency.Id,
            PaymentTypeId: paymentType.Id,
            InitialBalance: 1000m,
            UserId: userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var response = result.Value;
        Assert.NotEqual(Guid.Empty, response.Id);

        // Verify account was persisted
        var createdAccount = await _dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == response.Id);

        Assert.NotNull(createdAccount);
        Assert.Equal("My First Account", createdAccount.Name);
        Assert.Equal(currency.Id, createdAccount.CurrencyId);
        Assert.Equal(paymentType.Id, createdAccount.PaymentTypeId);
        Assert.Equal(1000m, createdAccount.InitialBalance);
        Assert.Equal(userId, createdAccount.UserId);
    }

    [Fact]
    public async Task Handle_WithInvalidCurrency_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var paymentType = new PaymentTypeFaker().Generate();
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAccountEndpoint.CommandHandler(_dbContext);
        var command = new CreateAccountEndpoint.Command(
            Name: "My Account",
            CurrencyId: Guid.NewGuid(), // Non-existent currency
            PaymentTypeId: paymentType.Id,
            InitialBalance: 1000m,
            UserId: userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Currency.NotFound, result.Errors.First());
    }

    [Fact]
    public async Task Handle_WithInvalidPaymentType_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        _dbContext.Currencies.Add(currency);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAccountEndpoint.CommandHandler(_dbContext);
        var command = new CreateAccountEndpoint.Command(
            Name: "My Account",
            CurrencyId: currency.Id,
            PaymentTypeId: Guid.NewGuid(), // Non-existent payment type
            InitialBalance: 1000m,
            UserId: userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.PaymentType.NotFound, result.Errors.First());
    }

    [Fact]
    public async Task Handle_WithMultipleAccounts_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateAccountEndpoint.CommandHandler(_dbContext);

        // Act - Create multiple accounts
        var command1 = new CreateAccountEndpoint.Command(
            Name: "Account 1",
            CurrencyId: currency.Id,
            PaymentTypeId: paymentType.Id,
            InitialBalance: 500m,
            UserId: userId);

        var command2 = new CreateAccountEndpoint.Command(
            Name: "Account 2",
            CurrencyId: currency.Id,
            PaymentTypeId: paymentType.Id,
            InitialBalance: 1500m,
            UserId: userId);

        var result1 = await handler.Handle(command1, CancellationToken.None);
        var result2 = await handler.Handle(command2, CancellationToken.None);

        // Assert
        Assert.False(result1.IsError);
        Assert.False(result2.IsError);

        var userAccounts = await _dbContext.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();

        Assert.Equal(2, userAccounts.Count);
        Assert.Single(userAccounts, a => a.Name == "Account 1");
        Assert.Single(userAccounts, a => a.Name == "Account 2");
    }
}
