using Budget.Tests.Utils;
using Budget.Tests.Utils.Records.Commands;
using Xunit;
using Budget.Domain.Common.Errors;
using Budget.Application.Records.Commands;

namespace Budget.Application.Tests.Records.Commands;

public class CreateRecordCommandTests
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
        var result = await handler.Handle(command!, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.True(result.Errors.FirstOrDefault().Code == Errors.User.NotFound.Code);
    }
}
