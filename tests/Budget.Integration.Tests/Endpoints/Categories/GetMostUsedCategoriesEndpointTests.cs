using Budget.Api.Endpoints.Categories;
using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Entities;
using Record = Budget.Api.Domain.Entities.Record;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Categories;

public class GetMostUsedCategoriesEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICacheManager _cacheManager;
    private readonly IMemoryCache _memoryCache;

    public GetMostUsedCategoriesEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
        _cacheManager = fixture.GetRequiredService<ICacheManager>();
        _memoryCache = fixture.GetRequiredService<IMemoryCache>();
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
    public async Task Handle_ShouldReturnMostUsedCategories()
    {
        // Arrange
        _memoryCache.Remove(CacheConstants.MostUsedCategories.Key);

        var userId = await CreateTestUserAsync();
        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        var category1 = new CategoryFaker()
            .RuleFor(c => c.Name, "Category 1")
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();
        var category2 = new CategoryFaker()
            .RuleFor(c => c.Name, "Category 2")
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var records = new List<Record>
        {
            new RecordFaker(account.Id, category1.Id).Generate(),
            new RecordFaker(account.Id, category1.Id).Generate(),
            new RecordFaker(account.Id, category1.Id).Generate(),
            new RecordFaker(account.Id, category2.Id).Generate()
        };

        _dbContext.Records.AddRange(records);
        await _dbContext.SaveChangesAsync();

        var handler = new GetMostUsedCategoriesEndpoint.QueryHandler(_dbContext, _cacheManager);
        var query = new GetMostUsedCategoriesEndpoint.Query(userId, 2);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var items = result.Value.ToList();
        Assert.Equal(2, items.Count);
        Assert.Equal(category1.Id, items[0].Id);
        Assert.Equal(3, items[0].Count);
        Assert.Equal(category2.Id, items[1].Id);
        Assert.Equal(1, items[1].Count);
    }
}
