using Budget.Tests.Utils;
using Budget.Tests.Utils.Categories.Queries;
using Xunit;

namespace Budget.Application.Tests.Categories.Queries;

public class GetAllPrimaryQueryTests
{
    [Fact]
    public async Task GetAllPrimaryAsync_ValidInput_ShouldReturnOneCategory()
    {
        // Arrange
        var handler = GetAllPrimaryQueryMockHelper.SetupHandler();
        var query = GetAllPrimaryQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task GetAllPrimaryAsync_IvalidUserId_ShouldReturnEmptyCollection()
    {
        // Arrange
        var handler = GetAllPrimaryQueryMockHelper.SetupHandler();
        var query = GetAllPrimaryQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}
