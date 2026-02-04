using Budget.Api.Endpoints.Categories;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Categories;

public class GetAllCategoriesEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllCategoriesEndpointTests(DatabaseFixtureApp fixture)
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

    [Fact]
    public async Task Handle_WithNoCategories_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetAllCategoriesEndpoint.QueryHandler(_dbContext);
        var query = new GetAllCategoriesEndpoint.Query(false, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WithPrimaryOnly_ShouldReturnOnlyPrimaryCategories()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var parentCategory = new CategoryFaker()
            .RuleFor(c => c.ParentCategoryId, _ => null)
            .RuleFor(c => c.Name, "Parent")
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        var subCategory = new CategoryFaker()
            .RuleFor(c => c.ParentCategoryId, parentCategory.Id)
            .RuleFor(c => c.Name, "Child")
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.AddRange(parentCategory, subCategory);
        await _dbContext.SaveChangesAsync();

        var handler = new GetAllCategoriesEndpoint.QueryHandler(_dbContext);
        var query = new GetAllCategoriesEndpoint.Query(true, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var categories = result.Value.ToList();
        Assert.Single(categories);
        Assert.Equal(parentCategory.Id, categories[0].Id);
    }
}
