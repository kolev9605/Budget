using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries;

public record GetAllRecordsQuery(
    string UserId,
    int PageNumber,
    int? PageSize) : IRequest<ErrorOr<IPagedListContainer<RecordModel>>>;

public class GetAllRecordsQueryHandler : IRequestHandler<GetAllRecordsQuery, ErrorOr<IPagedListContainer<RecordModel>>>
{
    private readonly IRecordRepository _recordRepository;

    public GetAllRecordsQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<IPagedListContainer<RecordModel>>> Handle(GetAllRecordsQuery query, CancellationToken cancellationToken)
    {
        var paginated = await _recordRepository.GetAllPaginatedAsync(query.UserId, query.PageNumber, query.PageSize ?? PaginationConstants.DefaultPageSize);

        return paginated.ToErrorOr();
    }
}

