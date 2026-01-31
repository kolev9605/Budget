using Budget.Api.Endpoints.Authentication;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Authentication;

public class RegisterEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
    }

    [Fact]
    public async Task Handle_WithNewUser_ShouldRegisterUser()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid()}@example.com";
        var password = "Password1!";

        var handler = new RegisterEndpoint.CommandHandler(_dbContext, _userManager);
        var command = new RegisterEndpoint.Command(email, password);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.UserId));

        var createdUser = await _userManager.FindByIdAsync(result.Value.UserId);
        Assert.NotNull(createdUser);
        Assert.Equal(email, createdUser!.Email);
    }

    [Fact]
    public async Task Handle_WithExistingUser_ShouldReturnError()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid()}@example.com";
        var password = "Password1!";
        var existingUser = new ApplicationUser { Email = email, UserName = email };

        var createResult = await _userManager.CreateAsync(existingUser, password);
        await _dbContext.SaveChangesAsync();
        if (!createResult.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        var handler = new RegisterEndpoint.CommandHandler(_dbContext, _userManager);
        var command = new RegisterEndpoint.Command(email, password);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.User.AlreadyExists.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldReturnError()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid()}@example.com";
        var invalidPassword = "weak";

        var handler = new RegisterEndpoint.CommandHandler(_dbContext, _userManager);
        var command = new RegisterEndpoint.Command(email, invalidPassword);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.User.AuthenticationFailed.Code, result.FirstError.Code);
    }
}
