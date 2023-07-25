
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Currencies;
using Budget.Domain.Models.PaymentTypes;
using Budget.Domain.Models.Records;
using Mapster;
using Moq;

namespace Budget.Tests.Utils
{
    public static class RepositoryMockHelper
    {
        // public static IRecordRepository SetupRecordRepository(Record record)
        // {
        //     var recordRepositoryMock = new Mock<IRecordRepository>() { DefaultValue = DefaultValue.Mock };

        //     recordRepositoryMock
        //         .Setup(x => x.CreateAsync(It.IsAny<Record>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(record));

        //     recordRepositoryMock
        //         .Setup(x => x.GetRecordByIdAsync<RecordModel>(DefaultValueConstants.Common.Id, It.IsAny<string>()))
        //         .Returns(Task.FromResult(record.Adapt<RecordModel>()));

        //     recordRepositoryMock
        //         .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
        //         .Returns(Task.FromResult(record));

        //     recordRepositoryMock
        //         .Setup(x => x.UpdateAsync(It.IsAny<Record>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(record));

        //     recordRepositoryMock
        //         .Setup(x => x.DeleteAsync(It.IsAny<Record>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(record));

        //     return recordRepositoryMock.Object;
        // }

        // public static IAccountRepository SetupAccountRepository(Account account)
        // {
        //     var accountRepositoryMock = new Mock<IAccountRepository>();

        //     accountRepositoryMock
        //         .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
        //         .Returns(Task.FromResult(account));

        //     accountRepositoryMock
        //         .Setup(x => x.GetByIdWithCurrencyAsync<TResult>(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(account));

        //     var accounts = new List<TResult> { account };

        //     accountRepositoryMock
        //         .Setup(x => x.GetAllByUserIdAsync<TResult>(DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(accounts.AsEnumerable()));

        //     accountRepositoryMock
        //         .Setup(x => x.CreateAsync<TResult>(It.IsAny<Account>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(account));

        //     accountRepositoryMock
        //         .Setup(x => x.UpdateAsync<TResult>(It.IsAny<Account>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(account));

        //     accountRepositoryMock
        //         .Setup(x => x.DeleteAsync<TResult>(It.IsAny<Account>(), It.IsAny<bool>()))
        //         .Returns(Task.FromResult(account));

        //     return accountRepositoryMock.Object;
        // }

        // public static ICategoryRepository SetupCategoryRepository(CategoryModel category)
        // {
        //     var categoryRepositoryMock = new Mock<ICategoryRepository>();

        //     categoryRepositoryMock
        //         .Setup(x => x.BaseGetByIdAsync<CategoryModel>(DefaultValueConstants.Common.Id))
        //         .Returns(Task.FromResult(category));

        //     categoryRepositoryMock
        //         .Setup(x => x.GetByIdWithSubcategoriesAsync<CategoryModel>(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(category));

        //     var categories = new List<CategoryModel> { category };

        //     categoryRepositoryMock
        //         .Setup(x => x.GetAllWithSubcategoriesAsync<CategoryModel>(DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(categories.AsEnumerable()));

        //     categoryRepositoryMock
        //         .Setup(x => x.GetAllPrimaryAsync<CategoryModel>(DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(categories.AsEnumerable()));

        //     categoryRepositoryMock
        //         .Setup(x => x.GetSubcategoriesByParentCategoryIdAsync<CategoryModel>(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
        //         .Returns(Task.FromResult(categories.AsEnumerable()));

        //     return categoryRepositoryMock.Object;
        // }

        // public static IRepository<PaymentType> SetupPaymentTypeRepository(PaymentTypeModel paymentType)
        // {
        //     var paymentTypeRepositoryMock = new Mock<IRepository<PaymentType>>();

        //     paymentTypeRepositoryMock
        //         .Setup(x => x.BaseGetByIdAsync<PaymentTypeModel>(DefaultValueConstants.Common.Id))
        //         .Returns(Task.FromResult(paymentType));

        //     var paymentTypes = new List<PaymentTypeModel> { paymentType };

        //     paymentTypeRepositoryMock
        //         .Setup(x => x.BaseGetAllAsync<PaymentTypeModel>())
        //         .Returns(Task.FromResult(paymentTypes.AsEnumerable()));

        //     return paymentTypeRepositoryMock.Object;
        // }

        // public static IRepository<Currency> SetupCurrencyRepository<TResult>(TResult currency)
        // {
        //     var currencyRepositoryMock = new Mock<IRepository<Currency>>();

        //     currencyRepositoryMock
        //         .Setup(x => x.BaseGetByIdAsync<TResult>(DefaultValueConstants.Common.Id))
        //         .Returns(Task.FromResult(currency));

        //     var currencies = new List<TResult> { currency };

        //     currencyRepositoryMock
        //         .Setup(x => x.BaseGetAllAsync<TResult>())
        //         .Returns(Task.FromResult(currencies.AsEnumerable()));

        //     return currencyRepositoryMock.Object;
        // }
    }
}
