using Budget.Application.Interfaces;
using Budget.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Budget.Tests.Core
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
    }
}
