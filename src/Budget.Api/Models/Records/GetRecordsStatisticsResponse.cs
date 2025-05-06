using Budget.Domain.Entities;

namespace Budget.Api.Models.Records;

public record GetRecordsStatisticsResponse(
    decimal Amount,
    DateTimeOffset RecordDate,
    RecordType RecordType,
    string CategoryName);
