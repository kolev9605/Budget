using Budget.Domain.Entities;
using Mapster;

namespace Budget.Domain.Models.Records.Statistics;

public record GetRecordsStatisticsResult(
    Guid Id,
    decimal Amount,
    DateTimeOffset RecordDate,
    RecordType RecordType,
    string CategoryName);


public static class GetRecordsStatisticsResultMappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<GetRecordsStatisticsResult, Record>
            .NewConfig()
            .Map(dest => dest.Category.Name, src => src.CategoryName);
    }
}
