using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class Currency
    {
        public static Error CurrencyNotFound => Error.NotFound(
            code: "Currency.NotFound",
            description: "Missing currency."
        );
    }
}
