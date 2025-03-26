using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Accounts.Commands;
using Xunit;

namespace Budget.Application.Tests.Accounts.Commands;

public class UpdateAccountCommandTests
{
    [Fact]
    public async Task UpdateAccount_ValidRequest_ShouldSucceed()
    {
        // Arrange
        var handler = UpdateAccountCommandMockHelper.SetupHandler();
        var command = UpdateAccountCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task UpdateAccounts_InvalidAccountId_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = UpdateAccountCommandMockHelper.SetupHandler();
        var command = UpdateAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }

    [Fact]
    public async Task UpdateAccounts_InvalidCurrencyId_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = UpdateAccountCommandMockHelper.SetupHandler();
        var command = UpdateAccountCommandMockHelper.SetupCommand(currencyId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Currency.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }

    [Fact]
    public async Task UpdateAccounts_InvalidUserId_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = UpdateAccountCommandMockHelper.SetupHandler();
        var command = UpdateAccountCommandMockHelper.SetupCommand(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }
}
