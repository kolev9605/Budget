using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class Record
    {
        public static Error NotFound => Error.NotFound(
            code: "Record.NotFound",
            description: "Record not found."
        );

        public static Error AccountsDoNotMatch => Error.Validation(
            code: "Record.AccountsDoNotMatch",
            description: "The account is not valid."
        );

        public static Error SameAccountsInTransfer => Error.Validation(
            code: "Record.TransferWithSameAccounts",
            description: "Transfers must be between two different accounts"
        );

        public static Error NoRecords => Error.NotFound(
            code: "Record.NoRecords",
            description: "There are no records for the given user."
        );
    }
}
