using System;
using System.Globalization;

namespace Budget.Domain.Exceptions
{
    public abstract class BudgetException : Exception
    {
        public BudgetException() : base() { }

        public BudgetException(string message) : base(message) { }

        public BudgetException(string message, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, message, args))
        {
        }
    }
}
