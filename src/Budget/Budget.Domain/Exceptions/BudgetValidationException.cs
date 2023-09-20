namespace Budget.Domain.Exceptions;

public class BudgetValidationException : BudgetException
{
    public BudgetValidationException()
    {
    }

    public BudgetValidationException(string message)
        : base(message)
    {
    }

    public BudgetValidationException(string message, params object[] args)
        : base(message, args)
    {
    }
}
