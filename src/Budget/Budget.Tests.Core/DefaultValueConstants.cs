﻿using Budget.Core.Entities;

namespace Budget.Tests.Core
{
    public static class DefaultValueConstants
    {
        public static class Record
        {
            public const RecordType Type = RecordType.Expense;
            public const decimal Amount = 20;
        }

        public static class User
        {
            public const string UserId = "user_id";
            public const string Username = "username";
            public const string InvalidId = "invalid_id";
        }

        public static class Common
        {
            public const int Id = 1;
            public const int InvalidId = 99;
        }

        public static class Category
        {
            public const CategoryType Type = CategoryType.Need;
        }

        public static class Account
        {
            public const int NumberOfAccounts = 1;
        }
    }
}
