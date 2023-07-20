using Budget.Domain.Constants;
using Budget.Domain.Exceptions;

namespace Budget.Domain.Guards
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
                throw new BudgetValidationException(string.Format(ValidationMessages.Common.IsNotNull, argumentName));
            }
        }

        public static void ValidateMaxtLength(string argumentValue, string argumentName, int maxLength)
        {
            if (argumentValue.Length > maxLength)
            {
                throw new BudgetValidationException(string.Format(ValidationMessages.Common.MaxLength, argumentName, maxLength));
            }
        }
    }
}
