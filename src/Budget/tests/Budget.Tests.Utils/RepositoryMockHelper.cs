
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Records;
using Mapster;
using Moq;

namespace Budget.Tests.Utils
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
                .Setup(x => x.GetRecordByIdMappedAsync(DefaultValueConstants.Common.Id, It.IsAny<string>()))
                .Returns(Task.FromResult(record.Adapt<RecordModel>()));

            recordRepositoryMock
                .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Record>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(record));

            recordRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Record>(), It.IsAny<bool>()))
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

            accountRepositoryMock
                .Setup(x => x.GetAccountModelByIdWithCurrencyAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(account.Adapt<AccountModel>()));

            var accounts = new List<Account> { account };

            accountRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(accounts.AsEnumerable()));

            accountRepositoryMock
                .Setup(x => x.GetAllAccountModelsByUserIdAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(accounts.Adapt<IEnumerable<AccountModel>>()));

            accountRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Account>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(account));

            accountRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Account>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(account));

            accountRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Account>(), It.IsAny<bool>()))
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
                .Setup(x => x.GetAllWithSubcategoriesCategoryModelsAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.Adapt<IEnumerable<CategoryModel>>()));

            categoryRepositoryMock
                .Setup(x => x.GetAllPrimaryCategoryModelsAsync(DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.Adapt<IEnumerable<CategoryModel>>()));

            categoryRepositoryMock
                .Setup(x => x.GetSubcategoriesByParentCategoryIdMappedAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(categories.Adapt<IEnumerable<CategoryModel>>()));

            categoryRepositoryMock
                .Setup(x => x.GetByIdWithSubcategoriesMappedAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
                .Returns(Task.FromResult(category.Adapt<CategoryModel>()));

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
                .Setup(x => x.BaseGetAllAsync())
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
                .Setup(x => x.BaseGetAllAsync())
                .Returns(Task.FromResult(currencies.AsEnumerable()));

            return currencyRepositoryMock.Object;
        }
    }
}
