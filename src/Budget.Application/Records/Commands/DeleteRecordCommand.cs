using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Records.Commands;

public record DeleteRecordCommand(
    Guid Id,
    string UserId) : IRequest<ErrorOr<RecordModel>>;

public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, ErrorOr<RecordModel>>
{
    public readonly IRecordRepository _recordRepository;

    public DeleteRecordCommandHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<RecordModel>> Handle(DeleteRecordCommand command, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetRecordByIdAsync(command.Id, command.UserId);

        if (record == null)
        {
            return Errors.Record.NotFound;
        }

        var existingTransferRecord = await _recordRepository.GetNegativeTransferRecordAsync(record);

        if (existingTransferRecord != null)
        {
            await _recordRepository.DeleteAsync(existingTransferRecord);
        }

        var deletedRecord = await _recordRepository.DeleteAsync(record);

        return deletedRecord.Adapt<RecordModel>();
    }
}
