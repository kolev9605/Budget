using Budget.Tests.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Infrastructure.Tests
{
    public class PaymentTypeServiceTest
    {
        [Fact]
        public async Task GetAllAsync_ValidInput_ShouldReturnOnePaymentType()
        {
            // Arrange
            var paymentTypeService = ServiceMockHelper.SetupPaymentTypeService();

            // Act
            var paymentTypes = await paymentTypeService.GetAllAsync();

            // Assert
            Assert.NotNull(paymentTypes);
            Assert.Single(paymentTypes);
        }
    }
}
