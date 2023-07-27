using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Models.Records;
using Budget.Tests.Utils;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Application.Tests
{
    public class RecordServiceTest
    {
        public RecordServiceTest()
        {
        }

        [Fact]
        public async Task CreateRecord_WithValidInputModel_ShouldSucceed()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = ModelMockHelper.CreateRecordModel();

            // Act
            var result = await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(model.Note, result.Note);
            Assert.Equal(model.AccountId, result.Account.Id);
            Assert.Equal(model.CategoryId, result.Category.Id);
            Assert.Equal(model.PaymentTypeId, result.PaymentType.Id);
            Assert.Equal(model.RecordType, result.RecordType);
        }

        [Fact]
        public async Task CreateRecord_WithInvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = ModelMockHelper.CreateRecordModel(accountId: DefaultValueConstants.Common.InvalidId);

            // Act
            var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task CreateRecord_WithInvalidCategoryId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new CreateRecordModel()
            {
                AccountId = DefaultValueConstants.Common.Id,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.InvalidId,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.Id,
                RecordType = RecordType.Expense
            };

            // Act
            var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task CreateRecord_WithInvalidPaymentTypeId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new CreateRecordModel()
            {
                AccountId = DefaultValueConstants.Common.Id,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.Id,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.InvalidId,
                RecordType = RecordType.Expense
            };

            // Act
            var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task CreateRecord_WithInvalidUserId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new CreateRecordModel()
            {
                AccountId = DefaultValueConstants.Common.Id,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.Id,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.Id,
                RecordType = RecordType.Expense
            };

            // Act
            var act = async () => await recordService.CreateAsync(model, "InvalidUser");

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task CreateRecord_PassNullModel_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            CreateRecordModel model = null;

            // Act
            var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task UpdateRecord_WithInvalidRecordId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new UpdateRecordModel()
            {
                Id = DefaultValueConstants.Common.InvalidId,
                AccountId = DefaultValueConstants.Common.Id,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.Id,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.Id,
                RecordType = RecordType.Expense
            };

            // Act
            var act = async () => await recordService.UpdateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }

        [Fact]
        public async Task UpdateRecord_WithValidInputModel_ShouldSucceed()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new UpdateRecordModel()
            {
                Id = DefaultValueConstants.Common.Id,
                AccountId = DefaultValueConstants.Common.Id,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.Id,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.Id,
                RecordType = RecordType.Expense
            };

            // Act
            var result = await recordService.UpdateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, result.Id);
        }

        [Fact]
        public async Task DeleteRecord_WithValidInputModel_ShouldSucceed()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            // Act
            var result = await recordService.DeleteAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(DefaultValueConstants.Common.Id, result.Id);
        }

        [Fact]
        public async Task DeleteRecord_WithInvalidRecordId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            // Act
            var act = async () => await recordService.DeleteAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }
    }
}
