using System.Text.Json;
using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Domain.Models.Import.Wallet;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Imports;

public class ImportWalletRecordsEndpoint : IEndpoint
{
    public class Request
    {
        public string WalletFileContent { get; set; } = null!;
    }

    public class Response
    {
        public int ImportedRecords { get; set; }
    }

    public record Command(
        string WalletFileContent,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class CommandHandler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWalletImportService _walletImportService;

        private readonly Dictionary<string, string> _walletCategoryMapping;
        private readonly Dictionary<string, RecordType> _walletRecordTypeMapping;

        public CommandHandler(
            BudgetDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            IWalletImportService walletImportService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _walletImportService = walletImportService;

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

        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var createdRecordsCount = await ImportWalletRecordsAsync(command.WalletFileContent, command.UserId, cancellationToken);
            return new Response { ImportedRecords = createdRecordsCount };
        }

        private async Task<int> ImportWalletRecordsAsync(string walletFileContent, string userId, CancellationToken cancellationToken)
        {
            var records = _walletImportService.ParseWalletCsv(walletFileContent);
            var paymentTypes = await _dbContext.PaymentTypes.ToListAsync(cancellationToken);
            var currencies = await _dbContext.Currencies.ToListAsync(cancellationToken);
            var debitCardPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Debit Card") ?? throw new ArgumentNullException();
            var cashPaymentType = paymentTypes.FirstOrDefault(pt => pt.Name == "Cash") ?? throw new ArgumentNullException();

            var insertedRecords = new List<Record>();

            foreach (var record in records)
            {
                var categoryFromDatabase = await MapCategoryAsync(record, cancellationToken);
                var account = await GetOrCreateAccountAsync(record, userId, currencies, debitCardPaymentType, cashPaymentType, cancellationToken);
                var recordType = MapRecordType(record);

                // The dates in the Wallet export are in local time
                var date = record.Date.ToUniversalTime();

                var recordToAdd = new Record(
                    record.Note,
                    date,
                    record.Amount,
                    account.Id,
                    null,
                    categoryFromDatabase.Id,
                    recordType,
                    _dateTimeProvider.UtcNowOffset);

                _dbContext.Records.Add(recordToAdd);
                insertedRecords.Add(recordToAdd);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Link transfers after all records are created
            await LinkTransfersAsync(userId, cancellationToken);

            return insertedRecords.Count;
        }

        private async Task LinkTransfersAsync(string userId, CancellationToken cancellationToken)
        {
            var allRecords = await _dbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .ToListAsync(cancellationToken);

            var transfers = allRecords
                .Where(r => r.RecordType == RecordType.Transfer)
                .GroupBy(r => r.RecordDate)
                .ToDictionary(r => r.Key, r => r.ToList());

            foreach (var transferPair in transfers)
            {
                var groupedByAmount = transferPair.Value
                    .GroupBy(r => Math.Abs(r.Amount));

                foreach (var group in groupedByAmount)
                {
                    var transferFromRecords = group.Where(r => r.Amount < 0).ToList();
                    var transferToRecords = group.Where(r => r.Amount > 0).ToList();

                    foreach (var transferFrom in transferFromRecords)
                    {
                        transferFrom.FromAccountId = transferToRecords.FirstOrDefault()?.AccountId;
                    }

                    foreach (var transferTo in transferToRecords)
                    {
                        transferTo.FromAccountId = transferFromRecords.FirstOrDefault()?.AccountId;
                    }
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
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
            return accountName == "Cash" ? cashPaymentType : debitCardPaymentType;
        }

        private async Task<Account> GetOrCreateAccountAsync(
            WalletImportModel record,
            string userId,
            IEnumerable<Currency> currencies,
            PaymentType debitCardPaymentType,
            PaymentType cashPaymentType,
            CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.UserId == userId && a.Name == record.Account)
                .FirstOrDefaultAsync(cancellationToken);

            if (account != null)
            {
                return account;
            }

            var currency = currencies.FirstOrDefault(c => c.Abbreviation == record.Currency);
            if (currency == null)
            {
                currency = currencies.FirstOrDefault(c => c.Abbreviation == "BGN") ?? throw new ArgumentNullException(nameof(currency));
            }

            var paymentType = MapPaymentType(debitCardPaymentType, cashPaymentType, record.Account);

            var accountToCreate = new Account
            {
                CurrencyId = currency.Id,
                InitialBalance = 0,
                Name = record.Account,
                UserId = userId,
                PaymentTypeId = paymentType.Id,
                CreatedOn = DateTimeOffset.UtcNow,
                UpdatedOn = DateTimeOffset.UtcNow,
            };

            _dbContext.Accounts.Add(accountToCreate);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return accountToCreate;
        }

        private async Task<Category> MapCategoryAsync(WalletImportModel record, CancellationToken cancellationToken)
        {
            var category = _walletCategoryMapping.GetValueOrDefault(record.Category);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var categoryFromDatabase = await _dbContext.Categories
                .Where(c => c.Name == category)
                .FirstOrDefaultAsync(cancellationToken);

            if (categoryFromDatabase == null)
            {
                throw new ArgumentNullException(nameof(categoryFromDatabase));
            }

            return categoryFromDatabase;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPost("/imports/wallet", async (
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(request.WalletFileContent, currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Imports");
    }
}
