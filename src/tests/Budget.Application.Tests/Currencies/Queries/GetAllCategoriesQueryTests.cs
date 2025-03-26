using Budget.Tests.Utils.Currencies.Queries;
using Xunit;

namespace Budget.Application.Tests.Currencies.Queries;

public class GetAllCategoriesQueryTests
{
    [Fact]
    public async Task GetAllAsync_ValidInput_ShouldReturnOneCurrency()
    {
        // Arrange
        var handler = GetAllCurrenciesQueryMockHelper.SetupHandler();
        var query = GetAllCurrenciesQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
    }
}
