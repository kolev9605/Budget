using Budget.Core.Interfaces;
using System;

namespace Budget.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime Today => DateTime.Today;
    }
}
