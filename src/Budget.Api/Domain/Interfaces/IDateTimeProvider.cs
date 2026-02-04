namespace Budget.Api.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }

    DateTime UtcToday { get; }

    DateTimeOffset UtcNowOffset { get; }
}
