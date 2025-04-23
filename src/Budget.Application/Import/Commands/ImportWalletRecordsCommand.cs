using System.Text.Json;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Import.Wallet;
using ErrorOr;
using MediatR;

namespace Budget.Application.Import.Commands;

public record ImportWalletRecordsCommand(
    string WalletFileContent,
    string UserId
) : IRequest<ErrorOr<int>>;


public class ImportWalletRecordsCommandHandler : IRequestHandler<ImportWalletRecordsCommand, ErrorOr<int>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IWalletImportService _walletImportService;
    private readonly IAccountRepository _accountRepository;
    private readonly IPaymentTypeRepository _paymentTypesRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRecordRepository _recordRepository;

    private readonly Dictionary<string, string> _walletCategoryMapping;
    private readonly Dictionary<string, RecordType> _walletRecordTypeMapping;

    public ImportWalletRecordsCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IWalletImportService walletImportService,
        IAccountRepository accountRepository,
        IPaymentTypeRepository paymentTypesRepository,
        ICurrencyRepository currencyRepository,
        ICategoryRepository categoryRepository,
        IRecordRepository recordRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _walletImportService = walletImportService;
        _accountRepository = accountRepository;
        _paymentTypesRepository = paymentTypesRepository;
        _currencyRepository = currencyRepository;
        _categoryRepository = categoryRepository;
        _recordRepository = recordRepository;

        // Load WalletCategoryMapping.json
        var categoryMappingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "WalletCategoryMapping.json");
        var categoryMappingJson = File.ReadAllText(categoryMappingFilePath);
        _walletCategoryMapping = JsonSerializer.Deserialize<Dictionary<string, string>>(categoryMappingJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        // Load WalletRecordTypeMapping.json
        var recordTypeMappingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "WalletRecordTypeMapping.json");
        var recordTypeMappingJson = File.ReadAllText(recordTypeMappingFilePath);
        var recordTypeMapping = JsonSerializer.Deserialize<Dictionary<string, string>>(recordTypeMappingJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        // Convert string values to RecordType enum
        _walletRecordTypeMapping = recordTypeMapping.ToDictionary(
            kvp => kvp.Key,
            kvp => Enum.Parse<RecordType>(kvp.Value, ignoreCase: true),
            StringComparer.InvariantCultureIgnoreCase
        );
    }

    public async Task<ErrorOr<int>> Handle(ImportWalletRecordsCommand request, CancellationToken cancellationToken)
    {
        var createdRecords = await ImportWalletRecordsAsync(request.WalletFileContent, request.UserId);
        return createdRecords.ToErrorOr();
    }
    private async Task<int> ImportWalletRecordsAsync(string walletFileContent, string userId)
    {
        var records = _walletImportService.ParseWalletCsv(walletFileContent);
        var paymentTypes = await _paymentTypesRepository.BaseGetAllAsync();
        var currencies = await _currencyRepository.BaseGetAllAsync();
        var debitCardPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Debit Card") ?? throw new ArgumentNullException();
        var cashPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Cash") ?? throw new ArgumentNullException();

        var insertedRecords = new List<Record>();

        foreach (var record in records)
        {
            var categoryFromDatabase = await MapCategoryAsync(record);
            var account = await GetOrCreateAccountAsync(record, userId, currencies, debitCardPaymentType, cashPaymentType);
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
                RecordDate = date,
                CreatedOn = _dateTimeProvider.UtcNow,
            };

            var createdRecord = await _recordRepository.CreateAsync(recordToAdd);
            insertedRecords.Add(createdRecord);
        }

        await LinkTransfersAsync(userId);

        return insertedRecords.Count;
    }

    private async Task LinkTransfersAsync(string userId)
    {
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
                var transferFromRecords = group.Where(r => r.Amount < 0) ?? new List<Record>();
                var transferToRecords = group.Where(r => r.Amount > 0) ?? new List<Record>();

                foreach (var transferFrom in transferFromRecords)
                {
                    transferFrom.FromAccountId = transferToRecords?.FirstOrDefault()?.AccountId;
                    await _recordRepository.UpdateAsync(transferFrom);
                }

                foreach (var transferTo in transferToRecords!)
                {
                    transferTo.FromAccountId = transferFromRecords.FirstOrDefault()?.AccountId;
                    await _recordRepository.UpdateAsync(transferTo);
                }
            }
        }
    }

    private RecordType MapRecordType(WalletImportModel record)
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

    private static PaymentType MapPaymentType(PaymentType debitCardPaymentType, PaymentType cashPaymentType, string accountName)
    {
        if (accountName == "Cash")
        {
            return cashPaymentType;
        }
        else
        {
            return debitCardPaymentType;
        }
    }

    private async Task<Account> GetOrCreateAccountAsync(
        WalletImportModel record,
        string userId,
        IEnumerable<Currency> currencies,
        PaymentType debitCardPaymentType,
        PaymentType cashPaymentType)
    {
        var account = await _accountRepository.GetByNameAsync(userId, record.Account);
        if (account != null)
        {
            return account;
        }
        else
        {
            var currency = currencies.FirstOrDefault(c => c.Abbreviation == record.Currency);
            if (currency == null)
            {
                currency = currencies.FirstOrDefault(c => c.Abbreviation == "BGN") ?? throw new ArgumentNullException(nameof(currency));
            }

            var paymentType = MapPaymentType(debitCardPaymentType, cashPaymentType, record.Account);

            var accountToCreate = new Account()
            {
                Currency = currency,
                InitialBalance = 0,
                Name = record.Account,
                UserId = userId,
                PaymentType = paymentType,
            };

            var createdAccount = await _accountRepository.CreateAsync(accountToCreate);

            return createdAccount;
        }
    }

    private async Task<Category> MapCategoryAsync(WalletImportModel record)
    {
        var category = _walletCategoryMapping.GetValueOrDefault(record.Category);

        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        var categoryFromDatabase = await _categoryRepository.GetByNameWithUsersAsync(category);

        if (categoryFromDatabase == null)
        {
            throw new ArgumentNullException(nameof(categoryFromDatabase));
        }

        return categoryFromDatabase;
    }
}
