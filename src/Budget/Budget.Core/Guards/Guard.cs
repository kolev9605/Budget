using Budget.Core.Constants;
using Budget.Core.Exceptions;

namespace Budget.Core.Guards
{
    public static class Guard
    {
        public static void IsNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new BudgetValidationException(string.Format(ValidationMessages.Common.IsNotNull, argumentName));
            }
        }

        public static void IsNotNullOrEmpty(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new BudgetValidationException(string.Format(ValidationMessages.Common.IsNotNullOrEmpty, argumentName));
            }
        }
    }
}
