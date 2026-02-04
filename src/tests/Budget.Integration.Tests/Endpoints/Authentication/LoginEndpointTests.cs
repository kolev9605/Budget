using Budget.Api.Endpoints.Authentication;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Infrastructure.Persistence;
using Budget.Integration.Tests.Fakers;
using Budget.Integration.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Budget.Integration.Tests.Endpoints.Authentication;

public class LoginEndpointTests : IClassFixture<DatabaseFixtureApp>
{
    private readonly BudgetDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginEndpointTests(DatabaseFixtureApp fixture)
    {
        _dbContext = fixture.GetRequiredService<BudgetDbContext>();
        _userManager = fixture.GetRequiredService<UserManager<ApplicationUser>>();
        _jwtTokenGenerator = fixture.GetRequiredService<IJwtTokenGenerator>();
    }

    private async Task<ApplicationUser> CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUserFaker()
            .RuleFor(u => u.Email, email)
            .RuleFor(u => u.UserName, email)
            .Generate();

        var result = await _userManager.CreateAsync(user, password);
        await _dbContext.SaveChangesAsync();

        if (!result.Succeeded)
        {
            throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user;
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid()}@example.com";
        var password = "Password1!";
        var user = await CreateUserAsync(email, password);

        var handler = new LoginEndpoint.QueryHandler(_jwtTokenGenerator, _userManager);
        var query = new LoginEndpoint.Query(email, password);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.Token));
        Assert.True(result.Value.ValidTo > DateTimeOffset.UtcNow.AddMinutes(-1));
        Assert.Equal(user.Email, email);
    }

    [Fact]
    public async Task Handle_WithNonexistentUser_ShouldReturnError()
    {
        // Arrange
        var handler = new LoginEndpoint.QueryHandler(_jwtTokenGenerator, _userManager);
        var query = new LoginEndpoint.Query("missing@example.com", "Password1!");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.User.NotFound.Code, result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldReturnError()
    {
        // Arrange
        var email = $"user-{Guid.NewGuid()}@example.com";
        var password = "Password1!";
        await CreateUserAsync(email, password);

        var handler = new LoginEndpoint.QueryHandler(_jwtTokenGenerator, _userManager);
        var query = new LoginEndpoint.Query(email, "WrongPassword1!");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.User.AuthenticationFailed.Code, result.FirstError.Code);
    }
}
