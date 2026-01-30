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

public class DeleteAccountEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteAccountEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<Account> CreateAccountAsync(string userId)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return account;
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldDeleteAccount()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var account = await CreateAccountAsync(userId);

        var handler = new DeleteAccountEndpoint.CommandHandler(_dbContext);
        var command = new DeleteAccountEndpoint.Command(account.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(account.Id, result.Value.Id);

        var deletedAccount = await _dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == account.Id);

        Assert.Null(deletedAccount);
    }

    [Fact]
    public async Task Handle_WithNonexistentAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();

        var handler = new DeleteAccountEndpoint.CommandHandler(_dbContext);
        var command = new DeleteAccountEndpoint.Command(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.Count == 1);
        Assert.True(result.Errors.FirstOrDefault() == Errors.Account.NotFound);
    }

    [Fact]
    public async Task Handle_WithDifferentUser_ShouldReturnError()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var otherUserId = await CreateTestUserAsync();
        var account = await CreateAccountAsync(ownerUserId);

        var handler = new DeleteAccountEndpoint.CommandHandler(_dbContext);
        var command = new DeleteAccountEndpoint.Command(account.Id, otherUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.Count == 1);
        Assert.True(result.Errors.FirstOrDefault() == Errors.Account.NotFound);

        var existingAccount = await _dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == account.Id);

        Assert.NotNull(existingAccount);
        Assert.NotEqual(otherUserId, existingAccount.UserId);
    }
}
