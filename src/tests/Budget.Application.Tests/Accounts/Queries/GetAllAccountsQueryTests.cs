using Budget.Tests.Utils;
using Budget.Tests.Utils.Accounts.Queries;
using Xunit;

namespace Budget.Application.Tests.Accounts.Queries;

public class GetAllAccountsQueryTests
{

    [Fact]
    public async Task GetAllAccounts_InvalidUserId_ShouldReturnEmptyCollection()
    {
        // Arrange
        var handler = GetAllAccountsQueryMockHelper.SetupHandler();
        var query = GetAllAccountsQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsError);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task GetAllAccounts_ValidUserId_ShouldReturnCorrectNumberOfAccounts()
    {
        // Arrange
        var handler = GetAllAccountsQueryMockHelper.SetupHandler();
        var query = GetAllAccountsQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsError);
        Assert.Single(result.Value);
    }
}
