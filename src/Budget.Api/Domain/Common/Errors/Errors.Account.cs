using ErrorOr;

namespace Budget.Api.Domain.Common.Errors;

public static partial class Errors
{
    public static class Account
    {
        public static Error NotFound => Error.NotFound(
            code: "Account.NotFound",
            description: "Missing account."
        );

        public static Error BelongsToAnotherUser => Error.Forbidden(
            code: "Account.AccountBelongsToAnotherUser",
            description: "The account belongs to another user."
        );

        public static Error HasRecords => Error.Validation(
            code: "Account.AccountHasRecords",
            description: "The account has records and cannot be deleted."
        );

        public static Error AlreadyExists => Error.Conflict(
            code: "Account.AlreadyExists",
            description: "The account already exists."
        );

        public static Error AlreadyDeactivated => Error.Validation(
            code: "Account.AlreadyDeactivated",
            description: "The account is already deactivated."
        );
    }
}
