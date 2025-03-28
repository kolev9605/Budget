using Budget.Tests.Utils.PaymentTypes.Queries;
using Xunit;

namespace Budget.Application.Tests.PaymentTypes.Queries;

public class GetAllPaymentTypesQueryTests
{
    [Fact]
    public async Task GetAllAsync_ValidInput_ShouldReturnOnePaymentType()
    {
        // Arrange
        var handler = GetAllPaymentTypesQueryMockHelper.SetupHandler();
        var query = GetAllPaymentTypesQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
    }
}
