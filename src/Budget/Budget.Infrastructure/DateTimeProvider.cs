using Budget.Domain.Interfaces;

namespace Budget.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime UtcToday => DateTime.UtcNow.Date;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

}
