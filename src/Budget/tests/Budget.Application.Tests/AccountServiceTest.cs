using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Accounts.Commands;
using Budget.Tests.Utils.Accounts.Queries;
using Humanizer;
using Xunit;

namespace Budget.Application.Tests;

public class AccountServiceTest
{
    // [Fact]
    // public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
    // {
    //     // Arrange
    //     var handler = GetAccountByIdQueryMockHelper.SetupHandler();
    //     var query = GetAccountByIdQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(DefaultValueConstants.Common.Id, result.Value.Id);
    // }

    // [Fact]
    // public async Task GetByIdAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = GetAccountByIdQueryMockHelper.SetupHandler();
    //     var query = GetAccountByIdQueryMockHelper.SetupQuery(id: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task GetAllAccountsAsync_InvalidUserId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = GetAllAccountsQueryMockHelper.SetupHandler();
    //     var query = GetAllAccountsQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.Empty(result.Value);
    // }

    // [Fact]
    // public async Task GetAllAccountsAsync_ValidUserId_ShouldReturnCorrectNumberOfAccounts()
    // {
    //     // Arrange
    //     var handler = GetAllAccountsQueryMockHelper.SetupHandler();
    //     var query = GetAllAccountsQueryMockHelper.SetupQuery();

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.Single(result.Value);
    // }

    // [Fact]
    // public async Task GetAllAccountsAsync_InvalidUserId_ShouldReturnZeroAccounts()
    // {
    //     // Arrange
    //     var handler = GetAllAccountsQueryMockHelper.SetupHandler();
    //     var query = GetAllAccountsQueryMockHelper.SetupQuery(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(query, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.Empty(result.Value);
    // }

    // [Fact]
    // public async Task CreateAccountAsync_ValidRequest_ShouldSucceed()
    // {
    //     // Arrange
    //     var handler = CreateAccountCommandMockHelper.SetupHandler();
    //     var command = CreateAccountCommandMockHelper.SetupCommand();

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    //     Assert.Equal(command.CurrencyId, result.Value.Currency.Id);
    //     Assert.Equal(command.InitialBalance, result.Value.InitialBalance);
    //     Assert.Equal(command.Name, result.Value.Name);
    // }

    // [Fact (Skip = "Missing validation")]
    // public async Task CreateAccountAsync_NullName_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = CreateAccountCommandMockHelper.SetupHandler();
    //     var command = CreateAccountCommandMockHelper.SetupCommand();
    //     // TODO: Figure out how to pass nulls to the mock helpers
    //     command = command with { Name = null! };

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    // }

    // [Fact]
    // public async Task UpdateAsync_ValidRequest_ShouldSucceed()
    // {
    //     // Arrange
    //     var handler = UpdateAccountCommandMockHelper.SetupHandler();
    //     var command = UpdateAccountCommandMockHelper.SetupCommand();

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    // }

    // [Fact]
    // public async Task UpdateAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = UpdateAccountCommandMockHelper.SetupHandler();
    //     var command = UpdateAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task UpdateAsync_InvalidCurrencyId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = UpdateAccountCommandMockHelper.SetupHandler();
    //     var command = UpdateAccountCommandMockHelper.SetupCommand(currencyId: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Currency.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task UpdateAsync_InvalidUserId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = UpdateAccountCommandMockHelper.SetupHandler();
    //     var command = UpdateAccountCommandMockHelper.SetupCommand(userId: DefaultValueConstants.User.InvalidId);

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task DeleteAsync_AttemptToDeleteAccountWithRecords_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = DeleteAccountCommandMockHelper.SetupHandler();
    //     var command = DeleteAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Account.AccountIdWithRecords);

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Account.HasRecords.Code, result.Errors.FirstOrDefault().Code);
    // }

    // [Fact]
    // public async Task DeleteAsync_ValidRequest_ShouldSucceed()
    // {
    //     // Arrange
    //     var handler = DeleteAccountCommandMockHelper.SetupHandler();
    //     var command = DeleteAccountCommandMockHelper.SetupCommand();

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.False(result.IsError);
    // }

    // [Fact]
    // public async Task DeleteAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
    // {
    //     // Arrange
    //     var handler = DeleteAccountCommandMockHelper.SetupHandler();
    //     var command = DeleteAccountCommandMockHelper.SetupCommand(id: DefaultValueConstants.Common.InvalidId);

    //     // Act
    //     var result = await handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     Assert.True(result.IsError);
    //     Assert.Equal(Errors.Account.NotFound.Code, result.Errors.FirstOrDefault().Code);
    // }
}
