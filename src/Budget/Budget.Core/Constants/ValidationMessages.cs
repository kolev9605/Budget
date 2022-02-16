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
    }
}
