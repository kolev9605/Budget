using Budget.Core.Exceptions;
using Budget.Core.Models.Records;
using Budget.Tests.Core;
using System.Threading.Tasks;
using Xunit;

namespace Budget.Infrastructure.Tests
{
    public class RecordServiceTest
    {

        [Fact]
        public async Task CreateRecord_WithValidInputModel_ShouldSucceed()
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
                RecordType = Core.Entities.RecordType.Expense
            };

            // Act
            var result = await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(result, DefaultValueConstants.Common.Id);
        }

        [Fact]
        public async Task CreateRecord_WithInvalidAccountId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            var model = new CreateRecordModel()
            {
                AccountId = DefaultValueConstants.Common.InvalidId,
                Amount = 20,
                CategoryId = DefaultValueConstants.Common.Id,
                Note = "test",
                PaymentTypeId = DefaultValueConstants.Common.Id,
                RecordType = Core.Entities.RecordType.Expense
            };

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
                RecordType = Core.Entities.RecordType.Expense
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
                RecordType = Core.Entities.RecordType.Expense
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
                RecordType = Core.Entities.RecordType.Expense
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
                RecordType = Core.Entities.RecordType.Expense
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
                RecordType = Core.Entities.RecordType.Expense
            };

            // Act
            var result = await recordService.UpdateAsync(model, DefaultValueConstants.User.UserId);

            // Assert
            Assert.Equal(result, DefaultValueConstants.Common.Id);
        }

        [Fact]
        public async Task DeleteRecord_WithValidInputModel_ShouldSucceed()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            // Act
            var result = await recordService.DeleteAsync(DefaultValueConstants.Common.Id);

            // Assert
            Assert.Equal(result, DefaultValueConstants.Common.Id);
        }

        [Fact]
        public async Task DeleteRecord_WithInvalidRecordId_ShouldThrowBudgetValidationException()
        {
            // Arrange
            var recordService = ServiceMockHelper.SetupRecordService();

            // Act
            var act = async () => await recordService.DeleteAsync(DefaultValueConstants.Common.InvalidId);

            // Assert
            var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
        }
    }
}
