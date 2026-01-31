using Budget.Api.Endpoints.Records;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Records;

public class GetRecordByIdEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetRecordByIdEndpointTests(DatabaseFixtureApp fixture)
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

    [Fact]
    public async Task Handle_WithValidInput_ShouldReturnRecord()
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

        var account = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var record = new RecordFaker(account.Id, category.Id).Generate();
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new GetRecordByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordByIdEndpoint.Query(record.Id, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(record.Id, result.Value.Id);
    }

    [Fact]
    public async Task Handle_WithNonexistentRecord_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetRecordByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetRecordByIdEndpoint.Query(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Record.NotFound.Code, result.FirstError.Code);
    }
}
