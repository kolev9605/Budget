using Budget.Core.Entities;
using Budget.Application.Interfaces.Repositories;
using Moq;

namespace Budget.Tests.Core
{
    public static class RepositoryMockHelper
    {
        public static IRecordRepository SetupRecordRepository(Record record)
        {
            var recordRepositoryMock = new Mock<IRecordRepository>();

            recordRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Record>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.GetRecordByIdAsync(DefaultValueConstants.Common.Id, It.IsAny<string>()))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Record>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.DeleteAsync(DefaultValueConstants.Common.Id, It.IsAny<bool>()))
                .Returns(Task.FromResult(record));

            return recordRepositoryMock.Object;
        }

        public static IAccountRepository SetupAccountRepository(Account account)
        {
            var accountRepositoryMock = new Mock<IAccountRepository>();

            accountRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(account));

            accountRepositoryMock
                .Setup(x => x.GetByIdWithCurrencyAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(account));

            var accounts = new List<Account> { account };

            accountRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(accounts.AsEnumerable()));

            accountRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Account>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(account));

            accountRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Account>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(account));

            accountRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(account));

            return accountRepositoryMock.Object;
        }

        public static ICategoryRepository SetupCategoryRepository(Category category)
        {
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            categoryRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(category));

            categoryRepositoryMock
                .Setup(x => x.GetByIdWithSubcategoriesAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(category));

            var categories = new List<Category> { category };

            categoryRepositoryMock
                .Setup(x => x.GetAllWithSubcategoriesAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.AsEnumerable()));

            categoryRepositoryMock
                .Setup(x => x.GetAllPrimaryAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.AsEnumerable()));

            categoryRepositoryMock
                .Setup(x => x.GetSubcategoriesByParentCategoryIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.AsEnumerable()));

            return categoryRepositoryMock.Object;
        }

        public static IRepository<PaymentType> SetupPaymentTypeRepository(PaymentType paymentType)
        {
            var paymentTypeRepositoryMock = new Mock<IRepository<PaymentType>>();

            paymentTypeRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(paymentType));

            var paymentTypes = new List<PaymentType> { paymentType };

            paymentTypeRepositoryMock
                .Setup(x => x.BaseAllAsync())
                .Returns(Task.FromResult(paymentTypes.AsEnumerable()));

            return paymentTypeRepositoryMock.Object;
        }

        public static IRepository<Currency> SetupCurrencyRepository(Currency currency)
        {
            var currencyRepositoryMock = new Mock<IRepository<Currency>>();

            currencyRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(currency));

            var currencies = new List<Currency> { currency };

            currencyRepositoryMock
                .Setup(x => x.BaseAllAsync())
                .Returns(Task.FromResult(currencies.AsEnumerable()));

            return currencyRepositoryMock.Object;
        }
    }
}
