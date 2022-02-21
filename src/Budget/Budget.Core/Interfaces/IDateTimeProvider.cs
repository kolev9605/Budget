using System;

namespace Budget.Core.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime Today { get; }
    }
}
