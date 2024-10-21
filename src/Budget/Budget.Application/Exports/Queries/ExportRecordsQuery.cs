using System.Text;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Exports.ExportRecords;
using ErrorOr;
using MediatR;
using Newtonsoft.Json;

namespace Budget.Application.Exports.Queries;

public record ExportRecordsQuery(string UserId) : IRequest<ErrorOr<ExportRecordsResult>>;

public class ExportRecordsQueryHandler : IRequestHandler<ExportRecordsQuery, ErrorOr<ExportRecordsResult>>
{
    private readonly IRecordRepository _recordRepository;

    public ExportRecordsQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<ExportRecordsResult>> Handle(ExportRecordsQuery query, CancellationToken cancellationToken)
    {
        var records = await _recordRepository.GetAllForExportAsync(query.UserId);

        // TODO: Why new JsonSerializerSettings()
        var result = JsonConvert.SerializeObject(records, new JsonSerializerSettings());

        var bytes =  Encoding.UTF8.GetBytes(result);

        return new ExportRecordsResult(bytes);
    }
}


