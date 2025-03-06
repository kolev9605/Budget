using Budget.Tests.Utils;
using Budget.Tests.Utils.Categories.Queries;
using Xunit;

namespace Budget.Application.Tests.Categories.Queries;

public class GetAllCategoriesQueryTests
{
    [Fact]
    public async Task GetAllAsync_ValidInput_ShouldReturnOneCategory()
    {
        // Arrange
        var handler = GetAllCategoriesQueryMockHelper.SetupHandler();
        var query = GetAllCategoriesQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);

    }

    [Fact]
    public async Task GetAllAsync_InvalidUserId_ShouldReturnEmptyCollection()
    {
        // Arrange
        var handler = GetAllCategoriesQueryMockHelper.SetupHandler();
        var query = GetAllCategoriesQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}
