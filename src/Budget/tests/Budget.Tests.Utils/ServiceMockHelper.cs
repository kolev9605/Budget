using Budget.Application.Services;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Currencies;
using Budget.Persistance;
using Budget.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Budget.Tests.Utils
{
    public class ServiceMockHelper
    {
        public static IDateTimeProvider SetupDateTimeProvider(DateTime? now = null)
        {
            if (now == null)
            {
                now = DateTime.UtcNow;
            }

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock
                .Setup(x => x.UtcNow)
                .Returns(now.Value);

            return dateTimeProviderMock.Object;
        }

        public static UserManager<ApplicationUser> SetupUserService(ApplicationUser user = null)
        {
            if (user == null)
            {
                user = EntityMockHelper.SetupUser();
            }

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            //userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<ApplicationUser, string>((x, y) => ls.Add(x));
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            return userManagerMock.Object;
        }

        public static RecordService SetupRecordService(
                    PaymentType paymentType,
                    Category category,
                    Account account,
                    Record record,
                    ApplicationUser user)
        {
            var recordService = new RecordService(
                SetupUserService(user),
                SetupDateTimeProvider(),
                RepositoryMockHelper.SetupRecordRepository(record),
                RepositoryMockHelper.SetupCategoryRepository(category),
                RepositoryMockHelper.SetupPaymentTypeRepository(paymentType),
                RepositoryMockHelper.SetupAccountRepository(account)
                );

            return recordService;
        }

        public static RecordService SetupRecordService()
        {
            var user = EntityMockHelper.SetupUser();
            var currency = EntityMockHelper.SetupCurrency();
            var paymentType = EntityMockHelper.SetupPaymentType();
            var category = EntityMockHelper.SetupCategory(user);
            var account = EntityMockHelper.SetupAccount(currency);
            var record = EntityMockHelper.SetupRecord(account, paymentType, category);

            var recordService = SetupRecordService(paymentType, category, account, record, user);

            return recordService;
        }

        public static AccountService SetupAccountService(Account account, Currency currency)
        {
            var accountService = new AccountService(
                RepositoryMockHelper.SetupAccountRepository(account),
                RepositoryMockHelper.SetupCurrencyRepository(currency)
                );

            return accountService;
        }

        public static AccountService SetupAccountService()
        {
            var currency = EntityMockHelper.SetupCurrency();

            var account = EntityMockHelper.SetupAccount(currency);

            var accountService = SetupAccountService(account, currency);

            return accountService;
        }

        public static CategoryService SetupCategoryService()
        {
            var user = EntityMockHelper.SetupUser();

            var category = EntityMockHelper.SetupCategory(user);

            var categoryService = new CategoryService(
                RepositoryMockHelper.SetupCategoryRepository(category));

            return categoryService;
        }

        public static CurrencyService SetupCurrencyService()
        {
            var currency = EntityMockHelper.SetupCurrency();

            var categoryService = new CurrencyService(
                RepositoryMockHelper.SetupCurrencyRepository(currency));

            return categoryService;
        }

        public static PaymentTypeService SetupPaymentTypeService()
        {
            var paymentType = EntityMockHelper.SetupPaymentType();

            var paymentTypeService = new PaymentTypeService(
                RepositoryMockHelper.SetupPaymentTypeRepository(paymentType));

            return paymentTypeService;
        }
    }
}
