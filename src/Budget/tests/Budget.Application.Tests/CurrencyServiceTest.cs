using Budget.Application.Services;
using Budget.Domain.Entities;
using Budget.Persistance;
using Budget.Persistance.Repositories;
using Budget.Tests.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class CurrencyServiceTest
    {

        public CurrencyServiceTest()
            : base()
        {
        }

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
}
