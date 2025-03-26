using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Accounts.Queries;
using Xunit;

namespace Budget.Application.Tests.Accounts.Queries;

public class GetAccountByIdQueryTests
{
    [Fact]
    public async Task GetAccountById_ValidInput_ShouldReturnValidResponse()
    {
        // Arrange
        var handler = GetAccountByIdQueryMockHelper.SetupHandler();
        var query = GetAccountByIdQueryMockHelper.SetupQuery();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(DefaultValueConstants.Common.Id, result.Value.Id);
    }

    [Fact]
    public async Task GetAccountById_InvalidAccountId_ShouldReturnErrorNotFound()
    {
        // Arrange
        var handler = GetAccountByIdQueryMockHelper.SetupHandler();
        var query = GetAccountByIdQueryMockHelper.SetupQuery(id: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }

    [Fact]
    public async Task GetAccountById_InvalidUserId_ShouldReturnErrorNotFound()
    {
        // Arrange
        var handler = GetAccountByIdQueryMockHelper.SetupHandler();
        var query = GetAccountByIdQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }
}
