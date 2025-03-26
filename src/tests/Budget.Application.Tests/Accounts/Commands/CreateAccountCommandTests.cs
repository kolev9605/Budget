using Budget.Tests.Utils.Accounts.Commands;
using Xunit;

namespace Budget.Application.Tests.Accounts.Commands;

public class CreateAccountCommandTests
{
    [Fact]
    public async Task CreateAccountAsync_ValidRequest_ShouldSucceed()
    {
        // Arrange
        var handler = CreateAccountCommandMockHelper.SetupHandler();
        var command = CreateAccountCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(command.CurrencyId, result.Value.Currency.Id);
        Assert.Equal(command.InitialBalance, result.Value.InitialBalance);
        Assert.Equal(command.Name, result.Value.Name);
    }

    [Fact (Skip = "Missing validation")]
    public async Task CreateAccountAsync_NullName_ShouldThrowBudgetValidationException()
    {
        // Arrange
        var handler = CreateAccountCommandMockHelper.SetupHandler();
        var command = CreateAccountCommandMockHelper.SetupCommand();
        // TODO: Figure out how to pass nulls to the mock helpers
        command = command with { Name = null! };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }
}
