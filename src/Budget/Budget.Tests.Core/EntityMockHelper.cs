using Budget.Core.Entities;

namespace Budget.Tests.Core
{
    public static class EntityMockHelper
    {
        public static Record SetupRecord(
            Account account,
            PaymentType paymentType,
            Category category,
            int id = DefaultValueConstants.Common.Id,
            RecordType recordType = DefaultValueConstants.Record.Type,
            decimal amount = DefaultValueConstants.Record.Amount)
        {
            var record = new Record()
            {
                Id = id,
                Account = account,
                PaymentType = paymentType,
                Amount = recordType == RecordType.Expense ? -Math.Abs(amount) : Math.Abs(amount),
                Category = category,
                DateCreated = DateTime.UtcNow,
                Note = $"Note{id}",
                RecordType = recordType
            };

            return record;

        }

        public static Account SetupAccount(
            Currency currency, 
            int id = DefaultValueConstants.Common.Id, 
            string userId = DefaultValueConstants.User.UserId)
        {
            var account = new Account()
            {
                Id = id,
                Currency = currency,
                Name = $"Account{id}",
                CurrencyId = currency.Id,
                UserId = userId
            };

            return account;
        }

        public static Category SetupCategory(
            int id = DefaultValueConstants.Common.Id, 
            CategoryType categoryType = DefaultValueConstants.Category.Type)
        {
            var category = new Category()
            {
                Id = id,
                Name = $"Category{id}",
                CategoryType = categoryType
            };

            return category;
        }

        public static PaymentType SetupPaymentType(int id = DefaultValueConstants.Common.Id)
        {
            var paymentType = new PaymentType()
            {
                Id = id,
                Name = $"PaymentType{id}"
            };

            return paymentType;
        }

        public static Currency SetupCurrency(int id = DefaultValueConstants.Common.Id)
        {
            var currency = new Currency()
            {
                Id = id,
                Name = $"Currency{id}",
                Abbreviation = $"Abbreviation{id}"
            };

            return currency;
        }

        public static ApplicationUser SetupUser(
            string id = DefaultValueConstants.User.UserId, 
            string username = DefaultValueConstants.User.Username)
        {
            var user = new ApplicationUser()
            {
                Id = id,
                UserName = username
            };

            return user;
        }
    }
}
