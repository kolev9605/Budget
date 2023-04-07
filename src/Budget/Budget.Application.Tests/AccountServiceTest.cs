using Budget.Application.Models.Accounts;
using Budget.Application.Services;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Persistance;
using Budget.Tests.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class AccountServiceTest : BaseTest
    {
        public AccountServiceTest()
            : base()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new BudgetDbContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                var currency = EntityMockHelper.SetupCurrency();
                var user = EntityMockHelper.SetupUser();
                var paymentType = EntityMockHelper.SetupPaymentType();
                var category = EntityMockHelper.SetupCategory(user);
                var account = EntityMockHelper.SetupAccount(currency, id: DefaultValueConstants.Account.AccountIdWithRecords);

                for (var i = 1; i <= 2; i++)
                {
                    context.Accounts.Add(EntityMockHelper.SetupAccount(currency, id: i));
                }

                for (var i = 1; i <= 3; i++)
                {
                    context.Records.Add(EntityMockHelper.SetupRecord(account, paymentType, category, i, RecordType.Expense, i * 10));
                }

                context.Currencies.Add(currency);
                context.Users.Add(user);
                context.PaymentTypes.Add(paymentType);
                context.Categories.Add(category);
                context.Accounts.Add(account);

                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetByIdAsync_ValidInput_ShouldReturnDefaultEntity()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            try
            {
                var account = await accountService.GetByIdAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);
                // Assert
                Assert.NotNull(account);
                Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [Fact]
        public async Task GetByIdAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            var act = async () => await accountService.GetByIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task GetAllAccountsAsync_InvalidUserId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            var act = async () => await accountService.GetByIdAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.InvalidId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ValidUserId_ShouldReturnCorrectNumberOfAccounts()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

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
            var context = CreateContext();
            var accountService = new AccountService(context);

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
            var context = CreateContext();
            var accountService = new AccountService(context);

            var createAccountRequest = new CreateAccountModel()
            {
                CurrencyId = DefaultValueConstants.Common.Id,
                InitialBalance = DefaultValueConstants.Account.DefaultInitialBalance,
                Name = DefaultValueConstants.Account.DefaultName
            };

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
            var context = CreateContext();
            var accountService = new AccountService(context);
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
            var context = CreateContext();
            var accountService = new AccountService(context);
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
            var context = CreateContext();
            var accountService = new AccountService(context);
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
            var context = CreateContext();
            var accountService = new AccountService(context);
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
            var context = CreateContext();
            var accountService = new AccountService(context);
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
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            var act = async () => await accountService.DeleteAccountAsync(DefaultValueConstants.Account.AccountIdWithRecords, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task DeleteAsync_ValidRequest_ShouldSucceed()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            var account =  await accountService.DeleteAccountAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, account.Id);
        }

        [Fact]
        public async Task DeleteAsync_InvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var context = CreateContext();
            var accountService = new AccountService(context);

            // Act
            var act = async () => await accountService.DeleteAccountAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }
    }
}
