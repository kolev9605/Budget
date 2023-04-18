using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Records;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Infrastructure.CsvModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Budget.Infrastructure.Services
{
    public class ImportService : IImportService
    {
        private readonly ICsvParser _csvParser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBudgetDbContext _budgetDbContext;
        private readonly Dictionary<string, string> _walletCategoryMapping;
        private readonly Dictionary<string, RecordType> _walletRecordTypeMapping;

        public ImportService(
            ICsvParser csvParser,
            IDateTimeProvider dateTimeProvider,
            IBudgetDbContext budgetDbContext)
        {
            _csvParser = csvParser;
            _dateTimeProvider = dateTimeProvider;
            _budgetDbContext = budgetDbContext;

            _walletCategoryMapping = new(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Food & Drinks", "Eating out" },
                { "Bar, cafe", "Bar Cafe" },
                { "Groceries", "Groceries" },
                { "Restaurant, fast-food", "Eating out" },

                { "Shopping", "Shopping" },
                { "Clothes & shoes", "Clothes" },
                { "Drug-store, chemist", "Medicaments" },
                { "Electronics, accessories", "Electronics" },
                { "Gifts, joy", "Gifts" },
                { "GF", "Partner" },
                { "Home, garden", "Housing" },
                { "Stationery, tools", "Stationery, tools" },
                { "Kids", "Kids" },
                { "Pets, animals", "Pets" },

                { "Housing", "Housing" },
                { "Energy, utilities", "Housing" },
                { "Maintenance, repairs", "Maintenance, repairs" },
                { "Mortgage", "Mortgage" },
                { "Services", "Services" },
                { "Rent", "Rent" },
                { "Furniture", "Furniture" },
                { "New house", "New house" },

                { "Transportation", "Transportation" },
                { "Public transport", "Public Transport" },
                { "Taxi", "Taxi" },

                { "Vehicle", "Car" },
                { "Fuel", "Fuel" },
                { "Parking", "Parking" },
                { "Vehicle insurance", "Car Insurance" },
                { "Vehicle maintenance", "Car Maintenance" },

                { "Life & Entertainment", "Life" },
                { "Active sport, fitness", "Sports" },
                { "Alcohol, tobacco", "Alcohol, tobacco" },
                { "Vape", "Vape" },
                { "Books, audio, subscriptions", "Books" },
                { "Charity, gifts", "Charity" },
                { "Culture, sport events", "Cinema" },
                { "Education, development", "Education" },
                { "Health care, doctor", "Healh Care" },
                { "Dentist", "Dentist" },
                { "Hobbies", "Hobbies" },
                { "Holiday, trips, hotels", "Holiday, trips, hotels" },
                { "Wellness, beauty", "Wellness, beauty" },
                { "Communication, PC", "Other" },

                { "Internet", "Internet" },
                { "Phone, mobile phone", "Phone, mobile phone" },
                { "Phone, cell phone", "Phone, mobile phone" },
                { "Software, apps, games", "Online Services" },

                { "Financial expenses", "Financial expenses" },
                { "Charges, Fees", "Charges & Fees" },
                { "Fines", "Fines" },
                { "Taxes", "Taxes" },

                {"Financial investments", "Investments"},

                { "Income", "Income" },
                { "Interests, dividends", "Interests, dividends" },
                { "Lending, renting", "Bank Loan" },
                { "Refunds (tax, purchase)", "Refunds (tax, purchase)" },
                { "Salary, income", "Salary" },
                { "Child Support", "Income" },

                { "Others", "Other" },
                { "Missing", "Missing" },
                { "TRANSFER", "Transfer" },
            };
            _walletRecordTypeMapping = new(StringComparer.InvariantCultureIgnoreCase)
            {
                { "income", RecordType.Income },
                { "expenses", RecordType.Expense },
                { "transfer", RecordType.Transfer }
            };
        }

        public async Task ImportRecordsAsync(string recordsFileJson, string userId)
        {
            var records = JsonConvert.DeserializeObject<IEnumerable<RecordsExportModel>>(recordsFileJson);
            if (records == null)
            {
                throw new BudgetValidationException();
            }

            var accounts = await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            var paymentTypes = await _budgetDbContext.PaymentTypes.ToListAsync();
            var categories = await _budgetDbContext.Categories.ToListAsync();

            var counter = 0;
            foreach (var recordModel in records)
            {
                var record = RecordsExportModel.ToRecord(recordModel);

                var account = accounts.FirstOrDefault(a => a.Name == recordModel.Account);
                if (account == null)
                {
                    throw new BudgetValidationException();
                }

                if (recordModel.FromAccount != null)
                {
                    var fromAccount = accounts.FirstOrDefault(a => a.Name == recordModel.FromAccount);
                    if (fromAccount == null)
                    {
                        throw new BudgetValidationException();
                    }

                    record.FromAccount = fromAccount;
                }

                var paymentType = paymentTypes.FirstOrDefault(pt => pt.Name == recordModel.PaymentType);
                if (paymentType == null)
                {
                    throw new BudgetValidationException();
                }

                var category = categories.FirstOrDefault(pt => pt.Name == recordModel.Category);
                if (category == null)
                {
                    throw new BudgetValidationException();
                }

                record.Account = account;
                record.PaymentType = paymentType;
                record.Category = category;

                await _budgetDbContext.Records.AddAsync(record);

                counter++;
                if (counter % 500 == 0)
                {
                    await _budgetDbContext.SaveChangesAsync();
                }
            }

            await _budgetDbContext.SaveChangesAsync();
        }

        public async Task<int> ImportWalletRecordsAsync(string walletFileContent, string userId)
        {
            var records = _csvParser.ParseCsvString<WalletCsvExportModel>(walletFileContent);
            var paymentTypes = await _budgetDbContext.PaymentTypes.ToListAsync();
            var currencies = await _budgetDbContext.Currencies.ToListAsync();
            var debitCardPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Debit Card");
            var cashPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Cash");

            var insertedRecords = new List<Record>();

            foreach (var record in records)
            {
                var categoryFromDatabase = await MapCategoryAsync(record);
                var account = await GetOrCreateAccountAsync(record, userId, currencies);
                var paymentType = MapPaymentType(debitCardPaymentType, cashPaymentType, account);
                var recordType = MapRecordType(record);

                // The dates in the Walled export are in local time
                var date = record.Date.ToUniversalTime();

                var recordToAdd = new Record()
                {
                    Account = account,
                    Category = categoryFromDatabase,
                    Note = record.Note,
                    Amount = record.Amount,
                    RecordType = recordType,
                    PaymentType = paymentType,
                    RecordDate = date,
                    DateCreated = _dateTimeProvider.Now,
                };

                var createdRecord = await _budgetDbContext.Records.AddAsync(recordToAdd);
                await _budgetDbContext.SaveChangesAsync();

                insertedRecords.Add(createdRecord.Entity);
            }

            await LinkTransfersAsync(userId);

            return insertedRecords.Count;
        }

        private async Task LinkTransfersAsync(string userId)
        {
            var allRecords = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ToListAsync();

            var transfers = allRecords
                .Where(r => r.RecordType == RecordType.Transfer)
                .GroupBy(r => new { r.RecordDate })
                .ToDictionary(r => r.Key, r => r.ToList());

            foreach (var transferPair in transfers)
            {
                var groupedByAmount = transferPair.Value
                    .GroupBy(r => Math.Abs(r.Amount));

                foreach (var group in groupedByAmount)
                {
                    var transferFromRecords = group.Where(r => r.Amount < 0);
                    var transferToRecords = group.Where(r => r.Amount > 0);

                    foreach (var transferFrom in transferFromRecords)
                    {
                        transferFrom.FromAccountId = transferToRecords.FirstOrDefault().AccountId;
                        _budgetDbContext.Records.Update(transferFrom);
                        await _budgetDbContext.SaveChangesAsync();
                    }

                    foreach (var transferTo in transferToRecords)
                    {
                        transferTo.FromAccountId = transferFromRecords.FirstOrDefault().AccountId;
                        _budgetDbContext.Records.Update(transferTo);
                        await _budgetDbContext.SaveChangesAsync();
                    }
                }
            }
        }

        private RecordType MapRecordType(WalletCsvExportModel record)
        {
            RecordType? recordType = null;
            if (record.Transfer)
            {
                recordType = RecordType.Transfer;
            }
            else
            {
                recordType = _walletRecordTypeMapping.GetValueOrDefault(record.Type);
            }

            if (recordType == null)
            {
                throw new ArgumentNullException(nameof(recordType));
            }

            return recordType.Value;
        }

        private static PaymentType MapPaymentType(PaymentType debitCardPaymentType, PaymentType cashPaymentType, Account account)
        {
            if (account.Name == "Cash")
            {
                return cashPaymentType;
            }
            else
            {
                return debitCardPaymentType;
            }
        }

        private async Task<Account> GetOrCreateAccountAsync(
            WalletCsvExportModel record,
            string userId,
            IEnumerable<Currency> currencies)
        {
            var account = await _budgetDbContext.Accounts
                .Where(a => a.UserId == userId)
                .Where(a => a.Name == record.Account)
                .FirstOrDefaultAsync();

            if (account != null)
            {
                return account;
            }
            else
            {
                var currency = currencies.FirstOrDefault(c => c.Abbreviation == record.Currency);

                if (currency == null)
                {
                    currency = currencies.FirstOrDefault(c => c.Abbreviation == "BGN");
                }

                var accountToCreate = new Account()
                {
                    Currency = currency,
                    InitialBalance = 0,
                    Name = record.Account,
                    UserId = userId
                };

                var createdAccount = await _budgetDbContext.Accounts.AddAsync(accountToCreate);
                await _budgetDbContext.SaveChangesAsync();

                return createdAccount.Entity;
            }
        }

        private async Task<Category> MapCategoryAsync(WalletCsvExportModel record)
        {
            var category = _walletCategoryMapping.GetValueOrDefault(record.Category);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var categoryFromDatabase = await _budgetDbContext.Categories
                .FirstOrDefaultAsync(c => c.Name == category);

            if (categoryFromDatabase == null)
            {
                throw new ArgumentNullException(nameof(categoryFromDatabase));
            }

            return categoryFromDatabase;
        }
    }
}
