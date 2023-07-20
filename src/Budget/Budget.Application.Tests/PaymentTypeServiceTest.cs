using Budget.Application.Services;
using Budget.Domain.Entities;
using Budget.Persistance;
using Budget.Persistance.Repositories;
using Budget.Tests.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class PaymentTypeServiceTest : BaseTest
    {
        public PaymentTypeServiceTest()
            :base()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new BudgetDbContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                var paymentType = EntityMockHelper.SetupPaymentType();

                context.PaymentTypes.Add(paymentType);

                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllAsync_ValidInput_ShouldReturnOnePaymentType()
        {
            // Arrange
            var context = CreateContext();
            var paymentTypeService = ServiceMockHelper.SetupPaymentTypeService(context);

            // Act
            var paymentTypes = await paymentTypeService.GetAllAsync();

            // Assert
            Assert.NotNull(paymentTypes);
            Assert.Single(paymentTypes);
        }
    }
}
