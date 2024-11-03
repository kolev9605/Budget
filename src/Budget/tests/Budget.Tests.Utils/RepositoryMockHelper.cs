
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.Currencies;
using Budget.Domain.Models.PaymentTypes;
using Budget.Domain.Models.Records;
using Budget.Domain.Models.Records.Create;
using Mapster;
using Moq;

namespace Budget.Tests.Utils;

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
            .Setup(x => x.GetForRecordCreationAsync(DefaultValueConstants.Common.Id))
            .Returns(Task.FromResult(account.Adapt<AccountForRecordCreationModel?>()));

        // accountRepositoryMock
        //     .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
        //     .Returns(Task.FromResult(account));

        // accountRepositoryMock
        //     .Setup(x => x.GetByIdWithCurrencyAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.Id))
        //     .Returns(Task.FromResult((Account?)account));

        var accountWithRecords = EntityMockHelper.SetupAccount(
            EntityMockHelper.SetupCurrency());

        var record = EntityMockHelper.SetupRecord(account, EntityMockHelper.SetupPaymentType(), EntityMockHelper.SetupCategory());

        accountWithRecords.Records.Add(record);

        accountRepositoryMock
            .Setup(x => x.GetByIdWithCurrencyAsync(It.IsAny<Guid>(), DefaultValueConstants.User.Id))
            // .Setup(x => x.GetByIdWithCurrencyAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.Id))
            .ReturnsAsync((Guid accountId, string userId) =>
            {
                if (accountId == DefaultValueConstants.Account.AccountIdWithRecords)
                {
                    return (Account?)accountWithRecords;
                }
                else if (accountId == DefaultValueConstants.Common.InvalidId)
                {
                    return null;
                }
                else
                {
                    return (Account?)account;
                }
            });

        // accountRepositoryMock
        //     .Setup(x => x.GetByIdWithCurrencyAsync(DefaultValueConstants.Account.AccountIdWithRecords, DefaultValueConstants.User.Id))
        //     .Returns(Task.FromResult((Account?)accountWithRecords));

        accountRepositoryMock
            .Setup(x => x.GetAccountModelByIdWithCurrencyAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(account.Adapt<AccountModel?>()));

        var accounts = new List<AccountModel> { account.Adapt<AccountModel>() };

        // accountRepositoryMock
        //     .Setup(x => x.GetAllByUserIdAsync(DefaultValueConstants.User.UserId))
        //     .Returns(Task.FromResult(accounts.AsEnumerable()));

        accountRepositoryMock
            .Setup(x => x.GetAllAccountModelsByUserIdAsync(DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(accounts.AsEnumerable()));

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
            .Setup(x => x.GetForRecordCreationAsync(DefaultValueConstants.Common.Id))
            .Returns(Task.FromResult(category.Adapt<CategoryForRecordCreationModel?>()));

        categoryRepositoryMock
            .Setup(x => x.GetByIdWithSubcategoriesMappedAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(category.Adapt<CategoryModel?>()));

        // categoryRepositoryMock
        //     .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
        //     .Returns(Task.FromResult(category));



        // categoryRepositoryMock
        //     .Setup(x => x.GetByIdWithSubcategoriesAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
        //     .Returns(Task.FromResult(category));

        var categories = new List<CategoryModel> { category.Adapt<CategoryModel>() };

        categoryRepositoryMock
            .Setup(x => x.GetAllWithSubcategoriesCategoryModelsAsync(DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        categoryRepositoryMock
            .Setup(x => x.GetAllPrimaryCategoryModelsAsync(DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        categoryRepositoryMock
            .Setup(x => x.GetSubcategoriesByParentCategoryIdMappedAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.Id))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        // categoryRepositoryMock
        //     .Setup(x => x.GetAllPrimaryCategoryModelsAsync(DefaultValueConstants.User.UserId))
        //     .Returns(Task.FromResult(categories.Adapt<IEnumerable<CategoryModel>>()));



        // categoryRepositoryMock
        //     .Setup(x => x.GetByIdWithSubcategoriesMappedAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId))
        //     .Returns(Task.FromResult(category.Adapt<CategoryModel>()));

        return categoryRepositoryMock.Object;
    }

    public static IPaymentTypeRepository SetupPaymentTypeRepository(PaymentType paymentType)
    {
        var paymentTypeRepositoryMock = new Mock<IPaymentTypeRepository>();

        paymentTypeRepositoryMock
            .Setup(x => x.GetForRecordCreationAsync(DefaultValueConstants.Common.Id))
            .Returns(Task.FromResult(paymentType.Adapt<PaymentTypeForRecordCreationModel?>()));

        // paymentTypeRepositoryMock
        //     .Setup(x => x.BaseGetByIdAsync(DefaultValueConstants.Common.Id))
        //     .Returns(Task.FromResult(paymentType));

        var paymentTypes = new List<PaymentTypeModel> { paymentType.Adapt<PaymentTypeModel>() };

        paymentTypeRepositoryMock
            .Setup(x => x.GetAllAsync())
            .Returns(Task.FromResult(paymentTypes.AsEnumerable()));

        return paymentTypeRepositoryMock.Object;
    }

    public static ICurrencyRepository SetupCurrencyRepository(Currency currency)
    {
        var currencyRepositoryMock = new Mock<ICurrencyRepository>();

        currencyRepositoryMock
            .Setup(x => x.GetByIdAsync(DefaultValueConstants.Common.Id))
            .Returns(Task.FromResult(currency.Adapt<CurrencyModel?>()));

        var currencies = new List<CurrencyModel> { currency.Adapt<CurrencyModel>() };

        currencyRepositoryMock
            .Setup(x => x.GetAllAsync())
            .Returns(Task.FromResult(currencies.AsEnumerable()));

        return currencyRepositoryMock.Object;
    }
}
