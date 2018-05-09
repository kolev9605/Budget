namespace Budget.Tests.Infrastructure
{
    using Budget.Data;
    using Budget.Infrastructure;
    using Budget.Infrastructure.ColorGenerator;
    using Budget.Services;
    using Budget.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ColorGeneratorTests
    {
        //[Fact]
        //public void Test()
        //{
        //    //Arrange
        //    var options = new DbContextOptionsBuilder<BudgetDbContext>()
        //        .UseInMemoryDatabase(databaseName: "FakeBudgetDbContext")
        //        .Options;

        //    var dbContext = new BudgetDbContext(options);
        //    ICategoryService categoryService = new CategoryService(dbContext);
        //    var colorGenerator = new ColorGenerator(categoryService);
        //    //Act
        //    //Assert
        //    Assert.True(true);
        //}
    }
}
