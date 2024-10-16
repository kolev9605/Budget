﻿using Budget.Domain.Entities;

namespace Budget.Tests.Utils;

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
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly Guid InvalidId = Guid.NewGuid();
    }

    public static class Category
    {
        public const CategoryType Type = CategoryType.Need;
    }

    public static class Account
    {
        public const int NumberOfAccounts = 1;
        public const decimal DefaultInitialBalance = 20m;
        public const string DefaultName = "account";
        public static readonly Guid AccountIdWithRecords = Guid.NewGuid();
    }
}
