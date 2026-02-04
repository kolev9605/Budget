using Budget.Api.Endpoints.Currencies;
using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Currencies;

public class GetAllCurrenciesEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly ICacheManager _cacheManager;
    private readonly IMemoryCache _memoryCache;

    public GetAllCurrenciesEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _cacheManager = fixture.GetRequiredService<ICacheManager>();
        _memoryCache = fixture.GetRequiredService<IMemoryCache>();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllCurrencies()
    {
        // Arrange
        _memoryCache.Remove(CacheConstants.Currencies.Key);

        var currency1 = new CurrencyFaker().Generate();
        var currency2 = new CurrencyFaker().Generate();
        _dbContext.Currencies.AddRange(currency1, currency2);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllCurrenciesEndpoint.QueryHandler(_dbContext, _cacheManager);
        var query = new GetAllCurrenciesEndpoint.Query();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var currencies = result.Value.ToList();
        Assert.True(currencies.Count >= 2);
        Assert.Contains(currencies, c => c.Id == currency1.Id);
        Assert.Contains(currencies, c => c.Id == currency2.Id);
    }
}
