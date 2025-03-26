using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Categories.Queries;
using Xunit;

namespace Budget.Application.Tests.Categories.Queries;

public class GetCategoryByIdQueryTests
{
    [Fact]
    public async Task GetCategoryById_ValidInput_ShouldReturnDefaultEntity()
    {
        // Arrange
        var handler = GetCategoryByIdQueryMockHelper.SetupHandler();
        var query = GetCategoryByIdQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(DefaultValueConstants.Common.Id, result.Value.Id);
    }

    [Fact]
    public async Task GetCategoryById_InvalidCategoryId_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = GetCategoryByIdQueryMockHelper.SetupHandler();
        var query = GetCategoryByIdQueryMockHelper.SetupQuery(categoryId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }
}
