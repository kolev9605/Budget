using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.CsvParser.CsvModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class ImportService : IImportService
    {
        private readonly ICsvParser _csvParser;
        private readonly IRecordRepository _recordRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<PaymentType> _paymentTypesRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly Dictionary<string, string> _walletCategoryMapping;
        private readonly Dictionary<string, RecordType> _walletRecordTypeMapping;

        public ImportService(
            ICsvParser csvParser,
            IRecordRepository recordRepository,
            ICategoryRepository categoryRepository,
            IAccountRepository accountRepository,
            IRepository<PaymentType> paymentTypesRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _csvParser = csvParser;
            _recordRepository = recordRepository;
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
            _paymentTypesRepository = paymentTypesRepository;
            _dateTimeProvider = dateTimeProvider;

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

                { "Transportation", "Transportation" },
                { "Public transport", "Public transport" },
                { "Taxi", "Taxi" },

                { "Vehicle", "Car" },
                { "Fuel", "Fuel" },
                { "Parking", "Parking" },
                { "Vehicle insurance", "Car insurance" },
                { "Vehicle maintenance", "Car maintenance" },

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

        public async Task<string> Parse(string userId)
        {
            userId = "45663575-b1e7-4717-b424-0c22c5862738";
            var records = _csvParser.ParseFromFile<WalletCsvExportModel>("wallet.csv");
            var paymentTypes = await _paymentTypesRepository.BaseAllAsync();
            var debitCardPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Debit Card");
            var cashPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Cash");

            var insertedRecords = new List<Record>();

            //foreach (var record in records)
            //{
            //    var category = _walletCategoryMapping.GetValueOrDefault(record.Category);

            //    if (category == null)
            //    {
            //        throw new ArgumentNullException(nameof(category));
            //    }

            //    var categoryFromDatabase = await _categoryRepository.GetByName(category);

            //    if (categoryFromDatabase == null)
            //    {
            //        throw new ArgumentNullException(nameof(categoryFromDatabase));
            //    }

            //    var account = await _accountRepository.GetByNameAsync(userId, record.Account);
            //    if (account == null)
            //    {
            //        throw new ArgumentNullException(nameof(account));
            //    }


            //    PaymentType paymentType = null;
            //    if (account.Name == "Cash")
            //    {
            //        paymentType = cashPaymentType;
            //    }
            //    else
            //    {
            //        paymentType = debitCardPaymentType;
            //    }

            //    RecordType? recordType = null;
            //    if (record.Transfer)
            //    {
            //        recordType = RecordType.Transfer;
            //    }
            //    else
            //    {                 
            //        recordType = _walletRecordTypeMapping.GetValueOrDefault(record.Type);
            //    }

            //    if (recordType == null)
            //    {
            //        throw new ArgumentNullException(nameof(recordType));
            //    }

            //    var recordToAdd = new Record()
            //    {
            //        Account = account,
            //        Category = categoryFromDatabase,
            //        Note = record.Note,
            //        Amount = record.Amount,
            //        RecordType = recordType.Value,
            //        PaymentType = paymentType,
            //        RecordDate = record.Date,
            //        DateCreated = _dateTimeProvider.Now,
            //    };

            //    var createdRecord = await _recordRepository.CreateAsync(recordToAdd);
            //    insertedRecords.Add(createdRecord);
            //}

            var allRecords = await _recordRepository.GetAllAsync(userId);

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
                        await _recordRepository.UpdateAsync(transferFrom);
                    }

                    foreach (var transferTo in transferToRecords)
                    {
                        transferTo.FromAccountId = transferFromRecords.FirstOrDefault().AccountId;
                        await _recordRepository.UpdateAsync(transferTo);
                    }
                }
            }

            return "ok";
        }
    }
}
