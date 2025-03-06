using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Budget.Tests.Utils;

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

    public static UserManager<ApplicationUser> SetupUserService(ApplicationUser? user = null)
    {
        if (user == null)
        {
            user = EntityMockHelper.SetupUser();
        }

        var store = new Mock<IUserStore<ApplicationUser>>();
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user ?? null));

        userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        return userManagerMock.Object;
    }

    public static ICacheManager SetupCacheManager<T>(T valueReturned)
    {
        var dateTimeProviderMock = new Mock<ICacheManager>();
        dateTimeProviderMock
            .Setup(x => x.GetOrCreateAsync<T>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Func<Task<T>>>()))
            .Returns(Task.FromResult(valueReturned));

        return dateTimeProviderMock.Object;
    }
}
