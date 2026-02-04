using Budget.Api.Endpoints.Categories;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Categories;

public class DeleteCategoryEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteCategoryEndpointTests(DatabaseFixtureApp fixture)
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
            .RuleFor(c => c.Users, _ => new List<UserCategory> { new UserCategory { UserId = userId } })
            .Generate();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    [Fact]
    public async Task Handle_WithSingleUserCategory_ShouldDeleteCategory()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var category = await CreateCategoryWithUserAsync(userId);

        var handler = new DeleteCategoryEndpoint.CommandHandler(_dbContext);
        var command = new DeleteCategoryEndpoint.Command(category.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        var deletedCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.Null(deletedCategory);
    }

    [Fact]
    public async Task Handle_WithMultipleUsers_ShouldRemoveUserOnly()
    {
        // Arrange
        var userId1 = await CreateTestUserAsync();
        var userId2 = await CreateTestUserAsync();

        var category = new CategoryFaker()
            .RuleFor(c => c.Users, _ => new List<UserCategory>
            {
                new UserCategory { UserId = userId1 },
                new UserCategory { UserId = userId2 }
            })
            .Generate();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var handler = new DeleteCategoryEndpoint.CommandHandler(_dbContext);
        var command = new DeleteCategoryEndpoint.Command(category.Id, userId1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);

        var updatedCategory = await _dbContext.Categories
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        Assert.NotNull(updatedCategory);
        Assert.DoesNotContain(updatedCategory!.Users, uc => uc.UserId == userId1);
        Assert.Contains(updatedCategory.Users, uc => uc.UserId == userId2);
    }

    [Fact]
    public async Task Handle_WithNonexistentCategory_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var handler = new DeleteCategoryEndpoint.CommandHandler(_dbContext);
        var command = new DeleteCategoryEndpoint.Command(Guid.NewGuid(), userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithCategoryHavingRecords_ShouldReturnError()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var category = await CreateCategoryWithUserAsync(userId);

        var currency = new CurrencyFaker().Generate();
        var paymentType = new PaymentTypeFaker().Generate();
        _dbContext.Currencies.Add(currency);
        _dbContext.PaymentTypes.Add(paymentType);
        await _dbContext.SaveChangesAsync();

        var account = new AccountFaker(userId, currency.Id, paymentType.Id).Generate();
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var record = new RecordFaker(account.Id, category.Id)
            .RuleFor(r => r.RecordType, RecordType.Expense)
            .Generate();

        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();

        var handler = new DeleteCategoryEndpoint.CommandHandler(_dbContext);
        var command = new DeleteCategoryEndpoint.Command(category.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.HasRecords.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithCategoryHavingSubCategories_ShouldReturnError()
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

        var handler = new DeleteCategoryEndpoint.CommandHandler(_dbContext);
        var command = new DeleteCategoryEndpoint.Command(parentCategory.Id, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Category.HasSubCategories.Code, result.FirstError.Code);
    }
}
