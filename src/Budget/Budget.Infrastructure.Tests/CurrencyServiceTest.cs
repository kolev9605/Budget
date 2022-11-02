using Budget.Tests.Core;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Infrastructure.Tests
{
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
    }
}
