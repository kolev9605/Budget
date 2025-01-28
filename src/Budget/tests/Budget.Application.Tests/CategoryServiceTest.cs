using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Categories.Queries;
using Xunit;

namespace Budget.Application.Tests;

public class CategoryServiceTest
{
    // [Fact]
    // public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
    // {
    //     // Arrange
    //     var handler = GetCategoryByIdQueryMockHelper.SetupHandler();
    //     var query = GetCategoryByIdQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(DefaultValueConstants.Common.Id, result.Value.Id);
    // }

    // [Fact]
    // public async Task GetByIdAsync_InvalidCategoryId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = GetCategoryByIdQueryMockHelper.SetupHandler();
    //     var query = GetCategoryByIdQueryMockHelper.SetupQuery(categoryId: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Category.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task GetAllAsync_ValidInput_ShouldReturnOneCategory()
    // {
    //     // Arrange
    //     var handler = GetAllCategoriesQueryMockHelper.SetupHandler();
    //     var query = GetAllCategoriesQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Single(result.Value);

    // }

    // [Fact]
    // public async Task GetAllAsync_IvalidUserId_ShouldReturnEmptyCollection()
    // {
    //     // Arrange
    //     var handler = GetAllCategoriesQueryMockHelper.SetupHandler();
    //     var query = GetAllCategoriesQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Empty(result.Value);
    // }

    // [Fact]
    // public async Task GetAllPrimaryAsync_ValidInput_ShouldReturnOneCategory()
    // {
    //     // Arrange
    //     var handler = GetAllPrimaryQueryMockHelper.SetupHandler();
    //     var query = GetAllPrimaryQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Single(result.Value);
    // }

    // [Fact]
    // public async Task GetAllPrimaryAsync_IvalidUserId_ShouldReturnEmptyCollection()
    // {
    //     // Arrange
    //     var handler = GetAllPrimaryQueryMockHelper.SetupHandler();
    //     var query = GetAllPrimaryQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Empty(result.Value);
    // }

    // [Fact]
    // public async Task GetAllSubcategoriesByParentCategoryIdAsync_ValidInput_ShouldReturnOneCategory()
    // {
    //     // Arrange
    //     var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
    //     var query = GetAllSubcategoriesQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Single(result.Value);
    // }

    // [Fact]
    // public async Task GetAllSubcategoriesByParentCategoryIdAsync_IvalidPrimaryCategoryId_ShouldReturnEmptyCollection()
    // {
    //     // Arrange
    //     var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
    //     var query = GetAllSubcategoriesQueryMockHelper.SetupQuery(parentCategoryId: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Empty(result.Value);
    // }

    // [Fact]
    // public async Task GetAllSubcategoriesByParentCategoryIdAsync_IvalidUserId_ShouldReturnEmptyCollection()
    // {
    //     // Arrange
    //     var handler = GetAllSubcategoriesQueryMockHelper.SetupHandler();
    //     var query = GetAllSubcategoriesQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Empty(result.Value);
    // }
}
