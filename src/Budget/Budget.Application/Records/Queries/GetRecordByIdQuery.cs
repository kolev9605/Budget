using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries;

public record GetRecordByIdQuery(
    Guid Id,
    string UserId) : IRequest<ErrorOr<RecordModel>>;

public class GetRecordByIdQueryHandler : IRequestHandler<GetRecordByIdQuery, ErrorOr<RecordModel>>
{
    private readonly IRecordRepository _recordRepository;

    public GetRecordByIdQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<RecordModel>> Handle(GetRecordByIdQuery query, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetRecordByIdMappedAsync(query.Id, query.UserId);

        if (record is null)
        {
            return Errors.Record.NotFound;
        }

        return record;
    }
}
