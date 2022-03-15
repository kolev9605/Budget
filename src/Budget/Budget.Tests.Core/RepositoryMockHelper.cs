using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
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
                .Setup(x => x.GetRecordByIdAsync(DefaultValueConstants.Common.Id))
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

            return accountRepositoryMock.Object;
        }

        public static IRepository<Category> SetupCategoryRepository(Category category)
        {
            var categoryRepositoryMock = new Mock<IRepository<Category>>();

            categoryRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(category));

            return categoryRepositoryMock.Object;
        }

        public static IRepository<PaymentType> SetupPaymentTypeRepository(PaymentType paymentType)
        {
            var paymentTypeRepositoryMock = new Mock<IRepository<PaymentType>>();

            paymentTypeRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(paymentType));

            return paymentTypeRepositoryMock.Object;
        }

        public static IRepository<Currency> SetupCurrencyRepository(Currency currency)
        {
            var currencyRepositoryMock = new Mock<IRepository<Currency>>();

            currencyRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(currency));

            return currencyRepositoryMock.Object;
        }
    }
}
