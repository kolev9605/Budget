using Budget.Application.Interfaces;
using System;

namespace Budget.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTime Today => DateTime.UtcNow.Date;
    }
}
