using Budget.Core.Exceptions;
using Budget.Tests.Core;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Infrastructure.Tests
{
    public class AccountServiceTest
    {
        [Fact]
        public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var account = await accountService.GetByIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var act = async () => await accountService.GetByIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task GetAllAccountsAsync_InvalidUserId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var act = async () => await accountService.GetByIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.InvalidId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ValidUserId_ShouldReturnCorrectNumberOfAccounts()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var accounts = await accountService.GetAllAccountsAsync(DefaultValueConstants.User.UserId);

            // Assert
            Assert.NotNull(accounts);
            Assert.Single(accounts);
        }

        [Fact]
        public async Task GetAllAccountsAsync_InvalidUserId_ShouldReturnZeroAccounts()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var accounts = await accountService.GetAllAccountsAsync(DefaultValueConstants.User.InvalidId);

            // Assert
            Assert.NotNull(accounts);
            Assert.Empty(accounts);
        }
    }
}
