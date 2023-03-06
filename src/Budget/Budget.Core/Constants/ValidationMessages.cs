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
            public const string UserWithIdDoesNotExists = "User with id {0} does not exists";
            public const string IncorrectPassword = "Incorrect password";
        }

        public static class Common
        {
            public const string IsNotNull = "{0} is required";
            public const string EntityDoesNotExist = "{0} does not exist";
            public const string MaxLength = "{0} must be less than {1} symbols";
        }

        public static class Accounts
        {
            public const string InvalidAccount = "The account {0} is not valid";
            public const string SameAccountsInTransfer = "Transfers must be between two different accounts";
        }

        public static class Admin
        {
            public const string CannotDeleteYourAccount = "You cannot delete your own account";
            public const string CannotChangeYourRole = "You cannot change your own role";
        }

        public static class CsvParser
        {
            public const string InvalidCsv = "The CSV is invalid";
        }

        public static class Categories
        {
            public const string AlreadyExist = "Category {0} already exist";
            public const string AlreadyDoesNotExist = "Category doesn't exist";
            public const string HasRecords = "Category {0} has records and cannot be deleted.";
            public const string HasSubCategoriesCannotBeDeleted = "Category {0} has sub-categories linked to it and cannot be deleted.";
            public const string HasSubCategoriesCannotBecomeSubcategory = "Category {0} has sub-categories linked to it and cannot be made sub-category itself.";

        }
    }
}
