using Budget.Api.Endpoints.Records;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Record = Budget.Api.Domain.Entities.Record;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class UpdateRecordEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateRecordEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
        _dateTimeProvider = fixture.GetRequiredService<IDateTimeProvider>();
    }

    private async Task<string> CreateTestUserAsync()
    {
        _dbContext.ChangeTracker.Clear();
        var user = new ApplicationUserFaker().Generate();
        var result = await _userManager.CreateAsync(user, "Password1!");

        await _dbContext.SaveChangesAsync();
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.Id;
    }

    private async Task<(Account account1, Account account2, Category category)> CreateAccountsAndCategoryAsync(string userId)
    {
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category = new CategoryFaker().Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var account1 = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        var account2 = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.AddRange(account1, account2);
        await _dbContext.SaveChangesAsync();

        return (account1, account2, category);
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldUpdateRecord()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow.AddDays(-1), -50m, account1.Id, null, category.Id, RecordType.Expense, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            -80m,
            account1.Id,
            category.Id,
            RecordType.Expense,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var updatedRecord = await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == record.Id);
        Assert.NotNull(updatedRecord);
        Assert.Equal("Updated", updatedRecord!.Note);
        Assert.Equal(-80m, updatedRecord.Amount);
    }

    [Fact]
    public async Task Handle_WithNonexistentRecord_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            Guid.NewGuid(),
            "Updated",
            100m,
            Guid.NewGuid(),
            Guid.NewGuid(),
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidAccount_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 50m, account1.Id, null, category.Id, RecordType.Income, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
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
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithAccountBelongingToAnotherUser_ShouldReturnError()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var otherUserId = await CreateTestUserAsync();
        var (ownerAccount, _, category) = await CreateAccountsAndCategoryAsync(ownerUserId);

        var otherCurrency = new CurrencyFaker().Generate();
        var otherPaymentType = new PaymentTypeFaker().Generate();
        _dbContext.Currencies.Add(otherCurrency);
        _dbContext.PaymentTypes.Add(otherPaymentType);
        await _dbContext.SaveChangesAsync();

        var otherAccount = new AccountFaker(otherUserId, otherCurrency.Id, otherPaymentType.Id).Generate();
        _dbContext.Accounts.Add(otherAccount);
        await _dbContext.SaveChangesAsync();

        var record = new Record("Old", DateTimeOffset.UtcNow, 50m, ownerAccount.Id, null, category.Id, RecordType.Income, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            100m,
            otherAccount.Id,
            category.Id,
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            ownerUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.BelongsToAnotherUser.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithNonexistentUser_ShouldReturnNotFound()
    {
        // Arrange
        var ownerUserId = await CreateTestUserAsync();
        var (account1, _, category) = await CreateAccountsAndCategoryAsync(ownerUserId);
        var missingUserId = Guid.NewGuid().ToString();

        var record = new Record("Old", DateTimeOffset.UtcNow, 50m, account1.Id, null, category.Id, RecordType.Income, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            100m,
            account1.Id,
            category.Id,
            RecordType.Income,
            DateTimeOffset.UtcNow,
            null,
            missingUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidCategory_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 50m, account1.Id, null, category.Id, RecordType.Income, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            100m,
            account1.Id,
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
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            150m,
            account1.Id,
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
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            150m,
            account1.Id,
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
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            150m,
            account1.Id,
            category.Id,
            RecordType.Transfer,
            DateTimeOffset.UtcNow,
            account1.Id,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.SameAccountsInTransfer.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithMissingTransferPair_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Old", DateTimeOffset.UtcNow, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var command = new UpdateRecordEndpoint.Command(
            record.Id,
            "Updated",
            150m,
            account1.Id,
            category.Id,
            RecordType.Transfer,
            DateTimeOffset.UtcNow,
            account2.Id,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithValidTransfer_ShouldUpdateBothRecords()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);
        var createdOn = _dateTimeProvider.UtcNowOffset;

        var positiveRecord = new Record("Transfer", createdOn, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, createdOn);
        var negativeRecord = positiveRecord.CreateNegativeTransferRecord();

        _dbContext.Records.AddRange(positiveRecord, negativeRecord);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateRecordEndpoint.CommandHandler(_dbContext, _dateTimeProvider, _userManager);
        var newRecordDate = createdOn.AddDays(1);
        var command = new UpdateRecordEndpoint.Command(
            positiveRecord.Id,
            "Updated Transfer",
            100m,
            account1.Id,
            category.Id,
            RecordType.Transfer,
            newRecordDate,
            account2.Id,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        var updatedPositive = await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == positiveRecord.Id);
        var updatedNegative = await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == negativeRecord.Id);

        Assert.NotNull(updatedPositive);
        Assert.NotNull(updatedNegative);
        Assert.Equal(100m, Math.Abs(updatedPositive!.Amount));
        Assert.Equal(100m, Math.Abs(updatedNegative!.Amount));
        Assert.Equal(newRecordDate, updatedPositive.RecordDate);
        Assert.Equal(newRecordDate, updatedNegative.RecordDate);
    }
}
