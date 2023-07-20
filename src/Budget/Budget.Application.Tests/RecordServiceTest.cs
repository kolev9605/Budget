// using Budget.Domain.Entities;
// using Budget.Domain.Exceptions;
// using Budget.Domain.Models.Records;
// using Budget.Persistance;
// using Budget.Tests.Core;
// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
// using Xunit;

// namespace Budget.Application.Tests
// {
//     public class RecordServiceTest : BaseTest
//     {
//         public RecordServiceTest()
//             : base()
//         {
//             // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
//             // at the end of the test (see Dispose below).
//             _connection = new SqliteConnection("Filename=:memory:");
//             _connection.Open();

//             // These options will be used by the context instances in this test suite, including the connection opened above.
//             _contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
//                 .UseSqlite(_connection)
//                 .Options;

//             // Create the schema and seed some data
//             using var context = new BudgetDbContext(_contextOptions);

//             if (context.Database.EnsureCreated())
//             {
//                 var currency = EntityMockHelper.SetupCurrency();
//                 var user = EntityMockHelper.SetupUser();
//                 var paymentType = EntityMockHelper.SetupPaymentType();
//                 var category = EntityMockHelper.SetupCategory(user);
//                 var account = EntityMockHelper.SetupAccount(currency, id: DefaultValueConstants.Account.AccountIdWithRecords);

//                 for (var i = 1; i <= 2; i++)
//                 {
//                     context.Accounts.Add(EntityMockHelper.SetupAccount(currency, id: i));
//                 }

//                 for (var i = 1; i <= 3; i++)
//                 {
//                     context.Records.Add(EntityMockHelper.SetupRecord(account, paymentType, category, i, RecordType.Expense, i * 10));
//                 }

//                 context.Currencies.Add(currency);
//                 context.Users.Add(user);
//                 context.PaymentTypes.Add(paymentType);
//                 context.Categories.Add(category);
//                 context.Accounts.Add(account);

//                 context.SaveChanges();
//             }
//         }

//         [Fact]
//         public async Task CreateRecord_WithValidInputModel_ShouldSucceed()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new CreateRecordModel()
//             {
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var result = await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             Assert.Equal(model.Note, result.Note);
//             Assert.Equal(model.AccountId, result.Account.Id);
//             Assert.Equal(model.CategoryId, result.Category.Id);
//             Assert.Equal(model.PaymentTypeId, result.PaymentType.Id);
//             Assert.Equal(model.RecordType, result.RecordType);
//         }

//         [Fact]
//         public async Task CreateRecord_WithInvalidAccountId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new CreateRecordModel()
//             {
//                 AccountId = DefaultValueConstants.Common.InvalidId,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact]
//         public async Task CreateRecord_WithInvalidCategoryId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new CreateRecordModel()
//             {
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.InvalidId,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact]
//         public async Task CreateRecord_WithInvalidPaymentTypeId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new CreateRecordModel()
//             {
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.InvalidId,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact]
//         public async Task CreateRecord_WithInvalidUserId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new CreateRecordModel()
//             {
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var act = async () => await recordService.CreateAsync(model, "InvalidUser");

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact]
//         public async Task CreateRecord_PassNullModel_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             CreateRecordModel model = null;

//             // Act
//             var act = async () => await recordService.CreateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact]
//         public async Task UpdateRecord_WithInvalidRecordId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new UpdateRecordModel()
//             {
//                 Id = DefaultValueConstants.Common.InvalidId,
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var act = async () => await recordService.UpdateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }

//         [Fact(Skip = "Math.Abs() doesn't work on SQLite")]
//         public async Task UpdateRecord_WithValidInputModel_ShouldSucceed()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             var model = new UpdateRecordModel()
//             {
//                 Id = DefaultValueConstants.Common.Id,
//                 AccountId = DefaultValueConstants.Common.Id,
//                 Amount = 20,
//                 CategoryId = DefaultValueConstants.Common.Id,
//                 Note = "test",
//                 PaymentTypeId = DefaultValueConstants.Common.Id,
//                 RecordType = RecordType.Expense
//             };

//             // Act
//             var result = await recordService.UpdateAsync(model, DefaultValueConstants.User.UserId);

//             // Assert
//             Assert.Equal(DefaultValueConstants.Common.Id, result.Id);
//         }

//         [Fact(Skip = "Math.Abs() doesn't work on SQLite")]
//         public async Task DeleteRecord_WithValidInputModel_ShouldSucceed()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             // Act
//             var result = await recordService.DeleteAsync(DefaultValueConstants.Common.Id, DefaultValueConstants.User.UserId);

//             // Assert
//             Assert.Equal(DefaultValueConstants.Common.Id, result.Id);
//         }

//         [Fact]
//         public async Task DeleteRecord_WithInvalidRecordId_ShouldThrowBudgetValidationException()
//         {
//             // Arrange
//             var context = CreateContext();
//             var recordService = ServiceMockHelper.SetupRecordService(context);

//             // Act
//             var act = async () => await recordService.DeleteAsync(DefaultValueConstants.Common.InvalidId, DefaultValueConstants.User.UserId);

//             // Assert
//             var exception = await Assert.ThrowsAsync<BudgetValidationException>(act);
//         }
//     }
// }
