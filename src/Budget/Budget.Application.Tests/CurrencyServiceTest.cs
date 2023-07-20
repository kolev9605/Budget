using Budget.Application.Services;
using Budget.Domain.Entities;
using Budget.Persistance;
using Budget.Persistance.Repositories;
using Budget.Tests.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class CurrencyServiceTest : BaseTest
    {

        // public CurrencyServiceTest()
        //     : base()
        // {
        //     // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        //     // at the end of the test (see Dispose below).
        //     _connection = new SqliteConnection("Filename=:memory:");
        //     _connection.Open();

        //     // These options will be used by the context instances in this test suite, including the connection opened above.
        //     _contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
        //         .UseSqlite(_connection)
        //         .Options;

        //     // Create the schema and seed some data
        //     using var context = new BudgetDbContext(_contextOptions);

        //     if (context.Database.EnsureCreated())
        //     {
        //         var currency = EntityMockHelper.SetupCurrency();

        //         context.Currencies.Add(currency);

        //         context.SaveChanges();
        //     }
        // }

        // [Fact]
        // public async Task GetAllAsync_ValidInput_ShouldReturnOneCurrency()
        // {
        //     // Arrange
        //     var context = CreateContext();
        //     var currencyService = ServiceMockHelper.SetupCurrencyService(context);

        //     // Act
        //     var currencies = await currencyService.GetAllAsync();

        //     // Assert
        //     Assert.NotNull(currencies);
        //     Assert.Single(currencies);
        // }

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
