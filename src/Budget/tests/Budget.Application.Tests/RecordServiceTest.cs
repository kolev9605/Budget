using Budget.Application.Records.Commands;
using Budget.Domain.Common.Errors;
using Budget.Tests.Utils;
using Budget.Tests.Utils.Records.Commands;
using Xunit;

namespace Budget.Application.Tests;

public class RecordServiceTest
{
    [Fact]
    public async Task CreateRecord_WithValidInputModel_ShouldSucceed()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        var command = CreateRecordCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        // TODO: More asserts can be added.
        Assert.False(result.IsError);
        Assert.Equal(command.Note, result.Value.Note);
        Assert.Equal(command.AccountId, result.Value.Account.Id);
        Assert.Equal(command.CategoryId, result.Value.Category.Id);
        Assert.Equal(command.PaymentTypeId, result.Value.PaymentType.Id);
        Assert.Equal(command.RecordType, result.Value.RecordType);
    }

    [Fact]
    public async Task CreateRecord_WithInvalidAccountId_ShouldReturnErrorCodeAccountNotFound()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        var command = CreateRecordCommandMockHelper.SetupCommand(accountId: DefaultValueConstants.Common.InvalidId);


        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.Account.NotFound.Code);
    }

    [Fact]
    public async Task CreateRecord_WithInvalidCategoryId_ShouldReturnErrorCodeCategoryNotFound()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        var command = CreateRecordCommandMockHelper.SetupCommand(categoryId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.Category.NotFound.Code);
    }

    [Fact]
    public async Task CreateRecord_WithInvalidPaymentTypeId_ShouldReturnErrorCodePaymentTypeNotFound()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        var command = CreateRecordCommandMockHelper.SetupCommand(paymentTypeId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.PaymentType.NotFound.Code);
    }

    [Fact]
    public async Task CreateRecord_WithInvalidUserId_ShouldReturnErrorCodeAccountBelongsToAnotherUser()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        var command = CreateRecordCommandMockHelper.SetupCommand(userId: DefaultValueConstants.User.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.Account.BelongsToAnotherUser.Code);
    }

    [Fact (Skip = "Validation not implemented")]
    public async Task CreateRecord_PassNullModel_TBD()
    {
        // Arrange
        var handler = CreateRecordCommandMockHelper.SetupHandler();
        CreateRecordCommand? command = null;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.User.NotFound.Code);
    }

    [Fact]
    public async Task UpdateRecord_WithInvalidRecordId_ShouldReturnErrorCodeRecordNotFound()
    {
        // Arrange
        var handler = UpdateRecordCommandMockHelper.SetupHandler();
        var command = UpdateRecordCommandMockHelper.SetupCommand(recordId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.Record.NotFound.Code);
    }

    [Fact]
    public async Task UpdateRecord_WithValidInputModel_ShouldSucceed()
    {
        // Arrange
        var handler = UpdateRecordCommandMockHelper.SetupHandler();
        var command = UpdateRecordCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.Value.Id == DefaultValueConstants.Common.Id);
    }

    [Fact]
    public async Task DeleteRecord_WithValidInputModel_ShouldSucceed()
    {
        // Arrange
        var handler = DeleteRecordCommandMockHelper.SetupHandler();
        var command = DeleteRecordCommandMockHelper.SetupCommand();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(DefaultValueConstants.Common.Id, result.Value.Id);
    }

    [Fact]
    public async Task DeleteRecord_WithInvalidRecordId_ShouldReturnErrorCodeRecordNotFound()
    {
        // Arrange
        var handler = DeleteRecordCommandMockHelper.SetupHandler();
        var command = DeleteRecordCommandMockHelper.SetupCommand(recordId: DefaultValueConstants.Common.InvalidId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(result.Errors.FirstOrDefault().Code, Errors.Record.NotFound.Code);

    }
}
