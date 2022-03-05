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
            public const string IsNotNull = "The argument {0} cannot be null";
            public const string IsNotNullOrEmpty = "The argument {0} cannot be null or empty";
            public const string EntityDoesNotExist = "{0} with id {1} does not exist";
        }

        public static class Accounts
        {
            public const string InvalidAccount = "The account {0} is not valid";
        }
    }
}
