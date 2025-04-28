using Budget.Domain.Entities;

namespace Budget.Domain.Models.Records.Statistics;

public record GetRecordsStatisticsResult(
    decimal Amount,
    DateTimeOffset RecordDate,
    RecordType RecordType);
