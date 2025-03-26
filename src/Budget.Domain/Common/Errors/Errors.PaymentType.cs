using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class PaymentType
    {
        public static Error NotFound => Error.NotFound(
            code: "PaymentType.NotFound",
            description: "Payment type not found."
        );
    }
}
