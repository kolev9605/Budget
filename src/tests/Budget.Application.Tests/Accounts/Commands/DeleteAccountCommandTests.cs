using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Accounts.Commands;
using Xunit;

namespace Budget.Application.Tests.Accounts.Commands;

public class DeleteAccountCommand
{
    [Fact]
    public async Task DeleteAsync_AttemptToDeleteAccountWithRecords_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = DeleteAccountCommandMockHelper.SetupHandler();
        var command = DeleteAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Account.AccountIdWithRecords);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.HasRecords.Code, result.Errors.FirstOrDefault().Code);
    }

    [Fact]
    public async Task DeleteAsync_ValidRequest_ShouldSucceed()
    {
        // Arrange
        var handler = DeleteAccountCommandMockHelper.SetupHandler();
        var command = DeleteAccountCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task DeleteAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = DeleteAccountCommandMockHelper.SetupHandler();
        var command = DeleteAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    }
}
