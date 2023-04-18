using Budget.Application.Interfaces;
using System;

namespace Budget.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime UtcToday => DateTime.UtcNow.Date;
    }
}
