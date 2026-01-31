using Budget.Api.Endpoints.Records;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class CreateRecordEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateRecordEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
        _dateTimeProvider = fixture.GetRequiredService<IDateTimeProvider>();
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

    private async Task<(Account account, Category category)> CreateAccountAndCategoryAsync(string userId)
    {
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

        return (account, category);
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldCreateRecord()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(userId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Test record",
            -50m,
            account.Id,
            category.Id,
            RecordType.Expense,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var createdRecord = await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == result.Value.Id);
        Assert.NotNull(createdRecord);
        Assert.Equal(account.Id, createdRecord!.AccountId);
        Assert.Equal(category.Id, createdRecord.CategoryId);
        Assert.Equal(RecordType.Expense, createdRecord.RecordType);
    }

    [Fact]
    public async Task Handle_WithMissingAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var category = new CategoryFaker().Generate();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Test",
            100m,
            Guid.NewGuid(),
            category.Id,
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithAccountBelongingToAnotherUser_ShouldReturnError()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var otherUserId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(ownerUserId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Test",
            100m,
            account.Id,
            category.Id,
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            otherUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.BelongsToAnotherUser.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithNonexistentUser_ShouldReturnBelongsToAnotherUser()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(ownerUserId);
        var missingUserId = Guid.NewGuid().ToString();

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Test",
            100m,
            account.Id,
            category.Id,
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            missingUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.BelongsToAnotherUser.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidCategory_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account, _) = await CreateAccountAndCategoryAsync(userId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Test",
            100m,
            account.Id,
            Guid.NewGuid(),
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithTransferMissingFromAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(userId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Transfer",
            100m,
            account.Id,
            category.Id,
            RecordType.Transfer,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithTransferFromAccountNotFound_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(userId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Transfer",
            100m,
            account.Id,
            category.Id,
            RecordType.Transfer,
            DateTimeOffset.UtcNow,
            Guid.NewGuid(),
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithTransferSameAccounts_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account, category) = await CreateAccountAndCategoryAsync(userId);

        var handler = new CreateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new CreateRecordEndpoint.Command(
            "Transfer",
            100m,
            account.Id,
            category.Id,
            RecordType.Transfer,
            DateTimeOffset.UtcNow,
            account.Id,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.SameAccountsInTransfer.Code, result.FirstError.Code);
    }
}
