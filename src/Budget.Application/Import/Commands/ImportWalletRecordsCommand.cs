// using Budget.Domain.Entities;
// using Budget.Domain.Interfaces;
// using ErrorOr;
// using MediatR;

// namespace Budget.Application.Import.Commands;

// public record ImportWalletRecordsCommand(
// ) : IRequest<ErrorOr<Record>>;


// public class ImportWalletRecordsCommandHandler : IRequestHandler<ImportWalletRecordsCommand, ErrorOr<Record>>
// {
//     private readonly ICsvParser _csvParser;
//     private readonly IDateTimeProvider _dateTimeProvider;

//     public ImportWalletRecordsCommandHandler(
//         ICsvParser csvParser,
//         IDateTimeProvider dateTimeProvider)
//     {
//         _csvParser = csvParser;
//         _dateTimeProvider = dateTimeProvider;
//     }

//     public async Task<ErrorOr<Record>> Handle(ImportWalletRecordsCommand request, CancellationToken cancellationToken)
//     {
//         throw new NotImplementedException();
//     }
//     public async Task<int> ImportWalletRecordsAsync(string walletFileContent, string userId)
//     {
//         var records = _csvParser.ParseCsvString<WalletCsvExportModel>(walletFileContent);
//         var paymentTypes = await _paymentTypesRepository.BaseGetAllAsync();
//         var currencies = await _currencyRepository.BaseGetAllAsync();
//         var debitCardPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Debit Card") ?? throw new ArgumentNullException();
//         var cashPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Cash") ?? throw new ArgumentNullException();

//         var insertedRecords = new List<Record>();

//         foreach (var record in records)
//         {
//             var categoryFromDatabase = await MapCategoryAsync(record);
//             var account = await GetOrCreateAccountAsync(record, userId, currencies);
//             var paymentType = MapPaymentType(debitCardPaymentType, cashPaymentType, account);
//             var recordType = MapRecordType(record);

//             // The dates in the Walled export are in local time
//             var date = record.Date.ToUniversalTime();

//             var recordToAdd = new Record()
//             {
//                 Account = account,
//                 Category = categoryFromDatabase,
//                 Note = record.Note,
//                 Amount = record.Amount,
//                 RecordType = recordType,
//                 PaymentType = paymentType,
//                 RecordDate = date,
//                 CreatedOn = _dateTimeProvider.UtcNow,
//             };

//             var createdRecord = await _recordRepository.CreateAsync(recordToAdd);
//             insertedRecords.Add(createdRecord);
//         }

//         await LinkTransfersAsync(userId);

//         return insertedRecords.Count;
//     }

//     private async Task LinkTransfersAsync(string userId)
//     {
//         var allRecords = await _recordRepository.GetAllAsync(userId);

//         var transfers = allRecords
//             .Where(r => r.RecordType == RecordType.Transfer)
//             .GroupBy(r => new { r.RecordDate })
//             .ToDictionary(r => r.Key, r => r.ToList());

//         foreach (var transferPair in transfers)
//         {
//             var groupedByAmount = transferPair.Value
//                 .GroupBy(r => Math.Abs(r.Amount));

//             foreach (var group in groupedByAmount)
//             {
//                 var transferFromRecords = group.Where(r => r.Amount < 0);
//                 var transferToRecords = group.Where(r => r.Amount > 0);

//                 foreach (var transferFrom in transferFromRecords)
//                 {
//                     transferFrom.FromAccountId = transferToRecords.FirstOrDefault().AccountId;
//                     await _recordRepository.UpdateAsync(transferFrom);
//                 }

//                 foreach (var transferTo in transferToRecords)
//                 {
//                     transferTo.FromAccountId = transferFromRecords.FirstOrDefault().AccountId;
//                     await _recordRepository.UpdateAsync(transferTo);
//                 }
//             }
//         }
//     }

//     private RecordType MapRecordType(WalletCsvExportModel record)
//     {
//         RecordType? recordType = null;
//         if (record.Transfer)
//         {
//             recordType = RecordType.Transfer;
//         }
//         else
//         {
//             recordType = _walletRecordTypeMapping.GetValueOrDefault(record.Type);
//         }

//         if (recordType == null)
//         {
//             throw new ArgumentNullException(nameof(recordType));
//         }

//         return recordType.Value;
//     }

//     private static PaymentType MapPaymentType(PaymentType debitCardPaymentType, PaymentType cashPaymentType, Account account)
//     {
//         if (account.Name == "Cash")
//         {
//             return cashPaymentType;
//         }
//         else
//         {
//             return debitCardPaymentType;
//         }
//     }

//     private async Task<Account> GetOrCreateAccountAsync(
//         WalletCsvExportModel record,
//         string userId,
//         IEnumerable<Currency> currencies)
//     {
//         var account = await _accountRepository.GetByNameAsync(userId, record.Account);
//         if (account != null)
//         {
//             return account;
//         }
//         else
//         {
//             var currency = currencies.FirstOrDefault(c => c.Abbreviation == record.Currency);

//             if (currency == null)
//             {
//                 currency = currencies.FirstOrDefault(c => c.Abbreviation == "BGN");
//             }

//             var accountToCreate = new Account()
//             {
//                 Currency = currency,
//                 InitialBalance = 0,
//                 Name = record.Account,
//                 UserId = userId
//             };

//             var createdAccount = await _accountRepository.CreateAsync(accountToCreate);

//             return createdAccount;
//         }
//     }

//     private async Task<Category> MapCategoryAsync(WalletCsvExportModel record)
//     {
//         var category = _walletCategoryMapping.GetValueOrDefault(record.Category);

//         if (category == null)
//         {
//             throw new ArgumentNullException(nameof(category));
//         }

//         var categoryFromDatabase = await _categoryRepository.GetByNameWithUsersAsync(category);

//         if (categoryFromDatabase == null)
//         {
//             throw new ArgumentNullException(nameof(categoryFromDatabase));
//         }

//         return categoryFromDatabase;
//     }
// }
