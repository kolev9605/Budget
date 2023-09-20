using Budget.Tests.Utils;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests;

public class CurrencyServiceTest
{
    [Fact]
    public async Task GetAllAsync_ValidInput_ShouldReturnOneCurrency()
    {
        // Arrange
        var currencyService = ServiceMockHelper.SetupCurrencyService();

        // Act
        var currencies = await currencyService.GetAllAsync();

        // Assert
        Assert.NotNull(currencies);
        Assert.Single(currencies);
    }

    [Fact]
    public async Task Test()
    {
        // Arrange
        var currencyService = ServiceMockHelper.SetupCurrencyService();

        // Act
        var currencies = await currencyService.GetAllAsync();

        // Assert
        Assert.NotNull(currencies);
        Assert.Single(currencies);
    }
}
