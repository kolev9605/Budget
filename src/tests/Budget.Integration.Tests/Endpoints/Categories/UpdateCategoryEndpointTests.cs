using Budget.Api.Endpoints.Categories;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Categories;

public class UpdateCategoryEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateCategoryEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
    }

    private async Task<string> CreateTestUserAsync()
    {
        var user = new ApplicationUserFaker().Generate();
        var result = await _userManager.CreateAsync(user, "Password1!");

        await _dbContext.SaveChangesAsync();
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.Id;
    }

    private async Task<Category> CreateCategoryWithUserAsync(string userId)
    {
        var category = new CategoryFaker()
            .RuleFor(c => c.Name, "Original Category")
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    [Fact]
    public async Task Handle_WithValidInput_ShouldUpdateCategory()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var category = await CreateCategoryWithUserAsync(userId);

        var handler = new UpdateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new UpdateCategoryEndpoint.Command(
            category.Id,
            "Updated Category",
            CategoryType.Income,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(category.Id, result.Value.Id);

        var updatedCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.NotNull(updatedCategory);
        Assert.Equal("Updated Category", updatedCategory!.Name);
        Assert.Equal(CategoryType.Income, updatedCategory.CategoryType);
        Assert.Null(updatedCategory.ParentCategoryId);
    }

    [Fact]
    public async Task Handle_WithNonexistentCategory_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new UpdateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new UpdateCategoryEndpoint.Command(
            Guid.NewGuid(),
            "Updated Category",
            CategoryType.Income,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WhenCategoryHasSubcategoriesAndParentSet_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var parentCategory = await CreateCategoryWithUserAsync(userId);
        var subCategory = new CategoryFaker()
            .RuleFor(c => c.ParentCategoryId, parentCategory.Id)
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.Add(subCategory);
        await _dbContext.SaveChangesAsync();

        var handler = new UpdateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new UpdateCategoryEndpoint.Command(
            parentCategory.Id,
            "Updated Parent",
            CategoryType.Need,
            Guid.NewGuid(),
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.CannotBecomeSubcategory.Code, result.FirstError.Code);
    }
}
