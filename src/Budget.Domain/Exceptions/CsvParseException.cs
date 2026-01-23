namespace Budget.Domain.Exceptions;

public class CsvParseException : BudgetException
{
    public CsvParseException()
    {
    }

    public CsvParseException(string message)
        : base(message)
    {
    }

    public CsvParseException(string message, params object[] args)
        : base(message, args)
    {
    }
}
