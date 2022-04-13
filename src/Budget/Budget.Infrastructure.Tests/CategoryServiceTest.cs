using Budget.Core.Exceptions;
using Budget.Tests.Core;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Infrastructure.Tests
{
    public class CategoryServiceTest
    {
        [Fact]
        public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var category = await categoryService.GetByIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(DefaultValueConstants.Common.Id, category.Id);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidCategoryId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var act = async () => await categoryService.GetByIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task GetAllAsync_ValidInput_ShouldReturnOneCategory()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllAsync(DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(categories);
            Assert.Single(categories);
        }

        [Fact]
        public async Task GetAllAsync_IvalidUserId_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllAsync(DefaultValueConstants.User.InvalidId);

            // Assert
            Assert.NotNull(categories);
            Assert.Empty(categories);
        }

        [Fact]
        public async Task GetAllPrimaryAsync_ValidInput_ShouldReturnOneCategory()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllPrimaryAsync(DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(categories);
            Assert.Single(categories);
        }

        [Fact]
        public async Task GetAllPrimaryAsync_IvalidUserId_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllPrimaryAsync(DefaultValueConstants.User.InvalidId);

            // Assert
            Assert.NotNull(categories);
            Assert.Empty(categories);
        }

        [Fact]
        public async Task GetAllSubcategoriesByParentCategoryIdAsync_ValidInput_ShouldReturnOneCategory()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllSubcategoriesByParentCategoryIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(categories);
            Assert.Single(categories);
        }

        [Fact]
        public async Task GetAllSubcategoriesByParentCategoryIdAsync_IvalidPrimaryCategoryId_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllSubcategoriesByParentCategoryIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(categories);
            Assert.Empty(categories);
        }

        [Fact]
        public async Task GetAllSubcategoriesByParentCategoryIdAsync_IvalidUserId_ShouldReturnEmptyCollection()
        {
            // Arrange
            var categoryService = ServiceMockHelper.SetupCategoryService();

            // Act
            var categories = await categoryService.GetAllSubcategoriesByParentCategoryIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.InvalidId);

            // Assert
            Assert.NotNull(categories);
            Assert.Empty(categories);
        }
    }
}
