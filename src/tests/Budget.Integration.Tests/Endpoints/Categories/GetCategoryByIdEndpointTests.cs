using Budget.Api.Endpoints.Categories;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Categories;

public class GetCategoryByIdEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetCategoryByIdEndpointTests(DatabaseFixtureApp fixture)
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
    public async Task Handle_WithValidInput_ShouldReturnCategory()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var category = new CategoryFaker()
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var handler = new GetCategoryByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetCategoryByIdEndpoint.Query(category.Id, userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(category.Id, result.Value.Id);
        Assert.Equal(category.Name, result.Value.Name);
    }

    [Fact]
    public async Task Handle_WithNonexistentCategory_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new GetCategoryByIdEndpoint.QueryHandler(_dbContext);
        var query = new GetCategoryByIdEndpoint.Query(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.NotFound.Code, result.FirstError.Code);
    }
}
