namespace Budget.Domain.Models.Records;

public record RecordsDateRangeResult(
    DateTimeOffset MinDate,
    DateTimeOffset MaxDate);
