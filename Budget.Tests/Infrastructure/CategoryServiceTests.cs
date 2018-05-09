namespace Budget.Tests.Infrastructure
{
    using Budget.Data;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services;
    using Budget.Services.Contracts;
    using Budget.Tests.TestUtils;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CategoryServiceTests
    {
        [Fact]
        public async Task AddOrGetCategoryAsync_AddNewCategory_ShouldAddCategory()
        {
            //Arrange
            var dbContext = Utils.GetDatabase();

            ICategoryService categoryService = new CategoryService(dbContext, null);

            //Act
            int id1 = await categoryService.AddOrGetCategoryAsync("TestCategory1", TransactionType.Expense, "");


            //Assert
            Assert.Equal(1, await dbContext.Categories.CountAsync());
            var cat1 = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id1);

            Assert.Equal("TestCategory1", cat1.Name);
            Assert.Equal(TransactionType.Expense, cat1.TransactionType);
        }

        [Fact]
        public async Task AddOrGetCategoryAsync_AddExistingCategory_ShouldReturnExistingCategory()
        {
            //Arrange
            var dbContext = Utils.GetDatabase();
            ICategoryService categoryService = new CategoryService(dbContext, null);

            //Act
            var firstInsertedId = await categoryService.AddOrGetCategoryAsync("TestCategory1", TransactionType.Income, "");
            var secondInsertedId = await categoryService.AddOrGetCategoryAsync("TestCategory1", TransactionType.Income, "");

            //Assert
            Assert.Equal(firstInsertedId, secondInsertedId);
        }

        [Fact]
        public async Task AddOrGetCategoryAsync_AddNewCategoryWithUser_ShouldAddInUserCategories()
        {
            //Arrange
            var dbContext = Utils.GetDatabase();
            var mockUserManager = Utils.GetMockedUserManager();

            const string UserId = "420";

            var user = new User()
            {
                Id = UserId,
                Email = "test@test.test"
            };

            mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            ICategoryService categoryService = new CategoryService(dbContext, mockUserManager.Object);

            //Act
            var insertedCategoryId = await categoryService.AddOrGetCategoryAsync("TestCategory1", TransactionType.Income, "", UserId);
            var userCategory = dbContext.UserCategories.FirstOrDefault(uc => uc.CategoryId == insertedCategoryId && uc.UserId == UserId);

            //Assert
            Assert.True(userCategory.UserId == UserId && userCategory.CategoryId == insertedCategoryId);
        }

        [Fact]
        public async Task AddOrGetCategoryAsync_AddNewCategoryWithInvalidUser_ShouldThrowException()
        {
            //Arrange
            var dbContext = Utils.GetDatabase();
            var mockUserManager = Utils.GetMockedUserManager();

            const string UserId = "420";

            var user = new User()
            {
                Id = UserId,
                Email = "test@test.test"
            };
            
            ICategoryService categoryService = new CategoryService(dbContext, mockUserManager.Object);

            //Act
            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => categoryService.AddOrGetCategoryAsync("TestCategory1", TransactionType.Income, "", UserId));
        }
    }
}
