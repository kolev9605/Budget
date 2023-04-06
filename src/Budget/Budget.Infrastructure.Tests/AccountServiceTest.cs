using Budget.Application.Models.Accounts;
using Budget.Core.Exceptions;
using Budget.Tests.Core;
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

        [Fact]
        public async Task CreateAccountAsync_ValidRequest_ShouldSucceed()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var createAccountRequest = new CreateAccountModel()
            {
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

            // Act
            var account = await accountService.CreateAccountAsync(createAccountRequest, DefaultValueConstants.User.UserId); ;

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
        }

        [Fact]
        public async Task CreateAccountAsync_NullName_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var createAccountRequest = new CreateAccountModel()
            {
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = null
            };

            // Act
            var act = async () => await accountService.CreateAccountAsync(createAccountRequest, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_ShouldSucceed()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var updateAccountRequest = new UpdateAccountModel()
            {
                Id = DefaultValueConstants.Common.Id,
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

            // Act
            var account = await accountService.UpdateAsync(updateAccountRequest, DefaultValueConstants.User.UserId); ;

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
        }

        [Fact]
        public async Task UpdateAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var updateAccountRequest = new UpdateAccountModel()
            {
                Id = DefaultValueConstants.Common.InvalidId,
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

            // Act
            var act = async () => await accountService.UpdateAsync(updateAccountRequest, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task UpdateAsync_InvalidCurrencyId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var updateAccountRequest = new UpdateAccountModel()
            {
                Id = DefaultValueConstants.Common.Id,
                CurrencyId = DefaultValueConstants.Common.InvalidId,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

            // Act
            var act = async () => await accountService.UpdateAsync(updateAccountRequest, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task UpdateAsync_InvalidUserId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();
            var updateAccountRequest = new UpdateAccountModel()
            {
                Id = DefaultValueConstants.Common.Id,
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

            // Act
            var act = async () => await accountService.UpdateAsync(updateAccountRequest, DefaultValueConstants.User.InvalidId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task DeleteAsync_ValidRequest_ShouldSucceed()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var account = await accountService.DeleteAccountAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId); ;

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
        }

        [Fact]
        public async Task DeleteAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var act = async () => await accountService.DeleteAccountAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }
    }
}
