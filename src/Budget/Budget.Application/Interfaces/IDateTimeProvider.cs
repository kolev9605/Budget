using System;

namespace Budget.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }

        DateTime UtcToday { get; }
    }
}
