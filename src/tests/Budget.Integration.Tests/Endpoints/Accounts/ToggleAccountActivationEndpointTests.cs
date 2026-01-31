using Budget.Api.Endpoints.Accounts;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Accounts;

public class ToggleAccountActivationEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ToggleAccountActivationEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<Account> CreateAccountWithDependenciesAsync(string userId, bool isActive = true)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id)
            .RuleFor(a => a.IsActive, isActive)
            .Generate();

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return account;
    }

    [Fact]
    public async Task Handle_WithActiveAccount_ShouldDeactivateAccount()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var account = await CreateAccountWithDependenciesAsync(userId, isActive: true);

        var handler = new ToggleAccountActivationEndpoint.CommandHandler(_dbContext);
        var command = new ToggleAccountActivationEndpoint.Command(account.Id, userId);

        Assert.True(account.IsActive);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        // Verify account was deactivated in database
        var updatedAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(updatedAccount);
        Assert.False(updatedAccount.IsActive);
    }

    [Fact]
    public async Task Handle_WithInactiveAccount_ShouldActivateAccount()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var account = await CreateAccountWithDependenciesAsync(userId, isActive: false);

        var handler = new ToggleAccountActivationEndpoint.CommandHandler(_dbContext);
        var command = new ToggleAccountActivationEndpoint.Command(account.Id, userId);

        Assert.False(account.IsActive);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        // Verify account was activated in database
        var updatedAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(updatedAccount);
        Assert.True(updatedAccount.IsActive);
    }

    [Fact]
    public async Task Handle_WithNonexistentAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new ToggleAccountActivationEndpoint.CommandHandler(_dbContext);
        var command = new ToggleAccountActivationEndpoint.Command(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Single(result.Errors);
        Assert.Equal(Errors.Account.NotFound, result.Errors.First());
    }

    [Fact]
    public async Task Handle_WithDifferentUser_ShouldReturnError()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var otherUserId = await CreateTestUserAsync();
        var account = await CreateAccountWithDependenciesAsync(ownerUserId, isActive: true);

        var handler = new ToggleAccountActivationEndpoint.CommandHandler(_dbContext);
        var command = new ToggleAccountActivationEndpoint.Command(account.Id, otherUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Single(result.Errors);
        Assert.Equal(Errors.Account.NotFound, result.Errors.First());
    }

    [Fact]
    public async Task Handle_CanToggleMultipleTimes()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var account = await CreateAccountWithDependenciesAsync(userId, isActive: true);

        var handler = new ToggleAccountActivationEndpoint.CommandHandler(_dbContext);

        // Act & Assert - Toggle 1 (Active -> Inactive)
        var result1 = await handler.Handle(
            new ToggleAccountActivationEndpoint.Command(account.Id, userId),
            CancellationToken.None);

        Assert.False(result1.IsError);

        var account1 = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(account1);
        Assert.False(account1.IsActive);

        // Act & Assert - Toggle 2 (Inactive -> Active)
        var result2 = await handler.Handle(
            new ToggleAccountActivationEndpoint.Command(account.Id, userId),
            CancellationToken.None);
        Assert.False(result2.IsError);

        var account2 = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(account2);
        Assert.True(account2.IsActive);

        // Act & Assert - Toggle 3 (Active -> Inactive)
        var result3 = await handler.Handle(
            new ToggleAccountActivationEndpoint.Command(account.Id, userId),
            CancellationToken.None);
        Assert.False(result3.IsError);

        var account3 = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(account3);
        Assert.False(account3.IsActive);
    }
}
