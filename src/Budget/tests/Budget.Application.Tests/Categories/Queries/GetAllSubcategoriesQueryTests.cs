using Budget.Tests.Utils;
using Budget.Tests.Utils.Categories.Queries;
using Xunit;

namespace Budget.Application.Tests.Categories.Queries;

public class GetAllSubcategoriesQueryTests
{
    [Fact]
    public async Task GetAllSubcategoriesByParentCategoryIdAsync_ValidInput_ShouldReturnOneCategory()
    {
        // Arrange
        var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
        var query = GetAllSubcategoriesQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task GetAllSubcategoriesByParentCategoryIdAsync_InvalidPrimaryCategoryId_ShouldReturnEmptyCollection()
    {
        // Arrange
        var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
        var query = GetAllSubcategoriesQueryMockHelper.SetupQuery(parentCategoryId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task GetAllSubcategoriesByParentCategoryIdAsync_InvalidUserId_ShouldReturnEmptyCollection()
    {
        // Arrange
        var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
        var query = GetAllSubcategoriesQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}
