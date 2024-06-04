using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class Account
    {
        public static Error AccountNotFound => Error.NotFound(
            code: "Account.NotFound",
            description: "Missing account."
        );

        public static Error AccountBelongsToAnotherUser => Error.Forbidden(
            code: "Account.AccountBelongsToAnotherUser",
            description: "The account belongs to another user."
        );

        public static Error AccountHasRecords => Error.Validation(
            code: "Account.AccountHasRecords",
            description: "The account has records and cannot be deleted."
        );
    }
}
