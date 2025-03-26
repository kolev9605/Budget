using Budget.Tests.Utils;
using Budget.Tests.Utils.Records.Commands;
using Budget.Domain.Common.Errors;
using Xunit;

namespace Budget.Application.Tests.Records.Commands;

public class DeleteRecordCommandTests
{
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
