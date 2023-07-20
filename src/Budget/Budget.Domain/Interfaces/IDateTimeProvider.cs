using System;

namespace Budget.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }

        DateTime UtcToday { get; }
    }
}
