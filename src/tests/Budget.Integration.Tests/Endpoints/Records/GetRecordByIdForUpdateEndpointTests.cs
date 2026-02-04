using Budget.Api.Endpoints.Records;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Record = Budget.Api.Domain.Entities.Record;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetRecordByIdForUpdateEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetRecordByIdForUpdateEndpointTests(DatabaseFixtureApp fixture)
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

    [Fact]
    public async Task Handle_WithTransferRecord_ShouldReturnPositiveTransferRecord()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
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

        var createdOn = _dateTimeProvider.UtcNowOffset;
        var positive = new Record("Transfer", createdOn, 100m, account1.Id, account2.Id, category.Id, RecordType.Transfer, createdOn);
        var negative = positive.CreateNegativeTransferRecord();

        _dbContext.Records.AddRange(positive, negative);
        await _dbContext.SaveChangesAsync();

        var handler = new GetRecordByIdForUpdateEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordByIdForUpdateEndpoint.Query(negative.Id, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(positive.Id, result.Value.Id);
        Assert.True(result.Value.Amount > 0);
    }

    [Fact]
    public async Task Handle_WithNonexistentRecord_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetRecordByIdForUpdateEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordByIdForUpdateEndpoint.Query(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }
}
