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

public class UpdateAccountEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateAccountEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<Account> CreateAccountWithDependenciesAsync(string userId, Guid currencyId, Guid paymentTypeId)
    {
        var account = new AccountFaker(userId, currencyId, paymentTypeId)
            .RuleFor(a => a.Name, "Original Account")
            .RuleFor(a => a.InitialBalance, 1000m)
            .Generate();

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return account;
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldUpdateAccount()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency1 = new CurrencyFaker().Generate();
        var currency2 = new CurrencyFaker().Generate();
        var paymentType1 = new PaymentTypeFaker().Generate();
        var paymentType2 = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.AddRange(currency1, currency2);
        _dbContext.PaymentTypes.AddRange(paymentType1, paymentType2);
        await _dbContext.SaveChangesAsync();

        var account = await CreateAccountWithDependenciesAsync(userId, currency1.Id, paymentType1.Id);

        var handler = new UpdateAccountEndpoint.CommandHandler(_dbContext);
        var command = new UpdateAccountEndpoint.Command(
            account.Id,
            "Updated Account",
            currency2.Id,
            paymentType2.Id,
            2500m,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(account.Id, result.Value.Id);

        var updatedAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
        Assert.NotNull(updatedAccount);
        Assert.Equal("Updated Account", updatedAccount.Name);
        Assert.Equal(currency2.Id, updatedAccount.CurrencyId);
        Assert.Equal(paymentType2.Id, updatedAccount.PaymentTypeId);
        Assert.Equal(2500m, updatedAccount.InitialBalance);
    }

    [Fact]
    public async Task Handle_WithNonexistentAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateAccountEndpoint.CommandHandler(_dbContext);
        var command = new UpdateAccountEndpoint.Command(
            Guid.NewGuid(),
            "Updated Account",
            currency.Id,
            paymentType.Id,
            2500m,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidCurrency_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = await CreateAccountWithDependenciesAsync(userId, currency.Id, paymentType.Id);

        var handler = new UpdateAccountEndpoint.CommandHandler(_dbContext);
        var command = new UpdateAccountEndpoint.Command(
            account.Id,
            "Updated Account",
            Guid.NewGuid(),
            paymentType.Id,
            2500m,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Currency.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidPaymentType_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = await CreateAccountWithDependenciesAsync(userId, currency.Id, paymentType.Id);

        var handler = new UpdateAccountEndpoint.CommandHandler(_dbContext);
        var command = new UpdateAccountEndpoint.Command(
            account.Id,
            "Updated Account",
            currency.Id,
            Guid.NewGuid(),
            2500m,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.PaymentType.NotFound.Code, result.FirstError.Code);
    }
}
