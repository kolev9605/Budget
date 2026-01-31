using Budget.Api.Endpoints.PaymentTypes;
using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Services;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.PaymentTypes;

public class GetAllPaymentTypesEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly ICacheManager _cacheManager;
    private readonly IMemoryCache _memoryCache;

    public GetAllPaymentTypesEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _cacheManager = fixture.GetRequiredService<ICacheManager>();
        _memoryCache = fixture.GetRequiredService<IMemoryCache>();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllPaymentTypes()
    {
        // Arrange
        _memoryCache.Remove(CacheConstants.PaymentTypes.Key);

        var paymentType1 = new PaymentTypeFaker().Generate();
        var paymentType2 = new PaymentTypeFaker().Generate();
        _dbContext.PaymentTypes.AddRange(paymentType1, paymentType2);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllPaymentTypesEndpoint.QueryHandler(_dbContext, _cacheManager);
        var query = new GetAllPaymentTypesEndpoint.Query();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var paymentTypes = result.Value.ToList();
        Assert.True(paymentTypes.Count >= 2);
        Assert.Contains(paymentTypes, p => p.Id == paymentType1.Id);
        Assert.Contains(paymentTypes, p => p.Id == paymentType2.Id);
    }
}
