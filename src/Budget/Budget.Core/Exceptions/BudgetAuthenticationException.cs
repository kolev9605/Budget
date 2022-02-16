namespace Budget.Core.Exceptions
{
    public class BudgetAuthenticationException : BudgetException
    {
        public BudgetAuthenticationException()
        {
        }

        public BudgetAuthenticationException(string message) 
            : base(message)
        {
        }

        public BudgetAuthenticationException(string message, params object[] args) 
            : base(message, args)
        {
        }
    }
}
