using Budget.Tests.Utils;
using Budget.Tests.Utils.Records.Commands;
using Xunit;
using Budget.Domain.Common.Errors;

namespace Budget.Application.Tests.Records.Commands;

public class UpdateRecordCommandTests
{
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
}
