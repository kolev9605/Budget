using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Budget.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Budget.Tests.Core
{
    public static class ServiceMockHelper
    {
        public static IDateTimeProvider SetupDateTimeProvider(DateTime? now = null)
        {
            if (now == null)
            {
                now = DateTime.UtcNow;
            }

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock
                .Setup(x => x.Now)
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
            Currency currency = null,
            PaymentType paymentType = null,
            Category category = null,
            Account account = null,
            Record record = null,
            ApplicationUser user = null)
        {
            if (currency == null)
            {
                currency = EntityMockHelper.SetupCurrency();
            }

            if (paymentType == null)
            {
                paymentType = EntityMockHelper.SetupPaymentType();
            }

            if (category == null)
            {
                category = EntityMockHelper.SetupCategory();
            }

            if (account == null)
            {
                account = EntityMockHelper.SetupAccount(currency);
            }

            if (record == null)
            {
                record = EntityMockHelper.SetupRecord(account, paymentType, category);
            }

            if (user == null)
            {
                user = EntityMockHelper.SetupUser();
            }

            var recordService = new RecordService(
                RepositoryMockHelper.SetupRecordRepository(record),
                RepositoryMockHelper.SetupAccountRepository(account),
                SetupDateTimeProvider(),
                RepositoryMockHelper.SetupCategoryRepository(category),
                RepositoryMockHelper.SetupPaymentTypeRepository(paymentType),
                SetupUserService(user)
                );

            return recordService;
        }
    }
}
