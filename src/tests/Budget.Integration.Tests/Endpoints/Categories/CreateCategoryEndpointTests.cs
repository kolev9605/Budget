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

public class CreateCategoryEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateCategoryEndpointTests(DatabaseFixtureApp fixture)
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

    private async Task<Category> CreateCategoryWithUserAsync(string userId, string name, CategoryType categoryType, Guid? parentCategoryId = null)
    {
        var category = new CategoryFaker()
            .RuleFor(c => c.Name, name)
            .RuleFor(c => c.CategoryType, categoryType)
            .RuleFor(c => c.ParentCategoryId, parentCategoryId)
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    [Fact]
    public async Task Handle_WithNewCategory_ShouldCreateCategory()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new CreateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new CreateCategoryEndpoint.Command(
            "New Category",
            CategoryType.Need,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        var createdCategory = await _dbContext.Categories
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == result.Value.Id);

        Assert.NotNull(createdCategory);
        Assert.Equal("New Category", createdCategory!.Name);
        Assert.Equal(CategoryType.Need, createdCategory.CategoryType);
        Assert.Contains(createdCategory.Users, uc => uc.UserId == userId);
    }

    [Fact]
    public async Task Handle_WithExistingCategoryForUser_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        await CreateCategoryWithUserAsync(userId, "Duplicate Category", CategoryType.Need);

        var handler = new CreateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new CreateCategoryEndpoint.Command(
            "Duplicate Category",
            CategoryType.Need,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.AlreadyExistsForUser.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithExistingCategoryDifferentType_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        await CreateCategoryWithUserAsync(userId, "Same Name", CategoryType.Need);

        var handler = new CreateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new CreateCategoryEndpoint.Command(
            "Same Name",
            CategoryType.Income,
            null,
            userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.AlreadyExists.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithExistingCategoryOtherUser_ShouldAttachUser()
    {
        // Arrange
        var userId1 = await CreateTestUserAsync();
        var userId2 = await CreateTestUserAsync();
        var category = await CreateCategoryWithUserAsync(userId1, "Shared Category", CategoryType.Need);

        var handler = new CreateCategoryEndpoint.CommandHandler(_dbContext);
        var command = new CreateCategoryEndpoint.Command(
            "Shared Category",
            CategoryType.Need,
            null,
            userId2);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(category.Id, result.Value.Id);

        var updatedCategory = await _dbContext.Categories
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        Assert.NotNull(updatedCategory);
        Assert.Contains(updatedCategory!.Users, uc => uc.UserId == userId1);
        Assert.Contains(updatedCategory.Users, uc => uc.UserId == userId2);
    }
}
