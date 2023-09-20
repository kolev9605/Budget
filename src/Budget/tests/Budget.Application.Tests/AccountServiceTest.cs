using Budget.Domain.Exceptions;
using Budget.Domain.Models.Accounts;
using Budget.Tests.Utils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
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
            Assert.True(accounts.Any());
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

            var createAccountRequest = ModelMockHelper.CreateAccountModel();

            // Act
            var account = await accountService.CreateAccountAsync(createAccountRequest, DefaultValueConstants.User.UserId); ;

            // Assert
            Assert.Equal(createAccountRequest.CurrencyId, account.Currency.Id);
            Assert.Equal(createAccountRequest.InitialBalance, account.InitialBalance);
            Assert.Equal(createAccountRequest.Name, account.Name);
        }

        [Fact]
        public async Task CreateAccountAsync_NullName_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            var createAccountRequest = ModelMockHelper.CreateAccountModel(name: null);

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

            var updatedInitialBalance = 999;
            var updatedName = "updated name";

            var updateAccountRequest = new UpdateAccountModel()
            {
                Id = DefaultValueConstants.Common.Id,
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = updatedInitialBalance,
                Name = updatedName
            };

            // Act
            var account = await accountService.UpdateAsync(updateAccountRequest, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(updateAccountRequest.Id, account.Id);
            Assert.Equal(updateAccountRequest.InitialBalance, account.InitialBalance);
            Assert.Equal(updateAccountRequest.Name, account.Name);
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
        public async Task DeleteAsync_AttemptToDeleteAccountWithRecords_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var act = async () => await accountService.DeleteAccountAsync(DefaultValueConstants.Account.AccountIdWithRecords, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task DeleteAsync_ValidRequest_ShouldSucceed()
        {
            // Arrange
            var accountService = ServiceMockHelper.SetupAccountService();

            // Act
            var account = await accountService.DeleteAccountAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

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
