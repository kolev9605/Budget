using Budget.Api.Endpoints.Records;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Record = Budget.Domain.Entities.Record;
using Budget.Domain.Interfaces;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class DeleteRecordEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteRecordEndpointTests(DatabaseFixtureApp fixture)
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
    public async Task Handle_WithValidRecord_ShouldDeleteRecord()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, _, category) = await CreateAccountsAndCategoryAsync(userId);

        var record = new Record("Note", DateTimeOffset.UtcNow, -25m, account1.Id, null, category.Id, RecordType.Expense, _dateTimeProvider.UtcNowOffset);
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new DeleteRecordEndpoint.CommandHandler(_dbContext);
        var command = new DeleteRecordEndpoint.Command(record.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var deleted = await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == record.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Handle_WithTransferRecord_ShouldDeletePair()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var (account1, account2, category) = await CreateAccountsAndCategoryAsync(userId);
        var createdOn = _dateTimeProvider.UtcNowOffset;

        var positive = new Record("Transfer", createdOn, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, createdOn);
        var negative = positive.CreateNegativeTransferRecord();

        _dbContext.Records.AddRange(positive, negative);
        await _dbContext.SaveChangesAsync();

        var handler = new DeleteRecordEndpoint.CommandHandler(_dbContext);
        var command = new DeleteRecordEndpoint.Command(positive.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Null(await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == positive.Id));
        Assert.Null(await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == negative.Id));
    }

    [Fact]
    public async Task Handle_WithNonexistentRecord_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new DeleteRecordEndpoint.CommandHandler(_dbContext);
        var command = new DeleteRecordEndpoint.Command(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }
}
