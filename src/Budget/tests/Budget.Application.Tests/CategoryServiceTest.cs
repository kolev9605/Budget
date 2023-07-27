using Budget.Domain.Exceptions;
using Budget.Tests.Utils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class CategoryServiceTest
    {
        public CategoryServiceTest()
            : base()
        {
            //// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            //// at the end of the test (see Dispose below).
            //_connection = new SqliteConnection("Filename=:memory:");
            //_connection.Open();

            //// These options will be used by the context instances in this test suite, including the connection opened above.
            //_contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
            //    .UseSqlite(_connection)
            //    .Options;

            //// Create the schema and seed some data
            //using var context = new BudgetDbContext(_contextOptions);

            //if (context.Database.EnsureCreated())
            //{
            //    var user = EntityMockHelper.SetupUser();
            //    var category = EntityMockHelper.SetupCategory(user, id: 10);

            //    for (var i = 1; i <= 2; i++)
            //    {
            //        var subCategory = EntityMockHelper.SetupCategory(user, i);
            //        subCategory.ParentCategory = category;

            //        context.Categories.Add(subCategory);
            //    }

            //    context.Categories.Add(category);

            //    context.SaveChanges();
            //}
        }

        [Fact]
        public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
        {
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
            Assert.True(categories.Any());
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
            Assert.True(categories.Any());
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
