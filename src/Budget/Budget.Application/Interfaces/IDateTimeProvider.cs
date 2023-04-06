using System;

namespace Budget.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime Today { get; }
    }
}
