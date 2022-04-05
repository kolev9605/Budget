namespace Budget.Core.Constants
{
    public static class ValidationMessages
    {
        public static class Authentication
        {
            public const string LoginFailed = "Login Failed";
            public const string RegisterFailed = "Register Failed";
            public const string UserExists = "User with username {0} already exists";
            public const string UserDoesNotExists = "User with username {0} does not exists";
            public const string IncorrectPassword = "Incorrect password";
        }

        public static class Common
        {
            public const string IsNotNull = "{0} is required";
            public const string EntityDoesNotExist = "{0} does not exist";
        }

        public static class Accounts
        {
            public const string InvalidAccount = "The account {0} is not valid";
            public const string SameAccountsInTransfer = "Transfers must be between two different accounts";

        }

        public static class CsvParser
        {
            public const string InvalidCsv = "The CSV is invalid";
        }
    }
}
