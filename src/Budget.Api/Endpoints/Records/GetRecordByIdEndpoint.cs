using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Helpers;
using Budget.Api.Infrastructure.Persistence;
using Budget.Api.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetRecordByIdEndpoint : IEndpoint
{
    public class Request
    {
        public Guid RecordId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public decimal Amount { get; set; }
        public AccountResponse Account { get; set; } = null!;
        public AccountResponse? FromAccount { get; set; }
        public CategoryResponse Category { get; set; } = null!;
        public RecordType RecordType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public class AccountResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public CurrencyResponse Currency { get; set; } = null!;

            public class CurrencyResponse
            {
                public Guid Id { get; set; }
                public string Abbreviation { get; set; } = null!;
            }
        }

        public class CategoryResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
        }
    }

    public record Query(
        Guid RecordId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var record = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Include(r => r.FromAccount)
                .Where(r => r.Id == query.RecordId)
                .Where(r => r.Account.UserId == query.UserId)
                .Select(r => new Response
                {
                    Id = r.Id,
                    Note = r.Note,
                    RecordDate = r.RecordDate,
                    Amount = r.Amount,
                    Account = new Response.AccountResponse
                    {
                        Id = r.Account.Id,
                        Name = r.Account.Name,
                        Currency = new Response.AccountResponse.CurrencyResponse
                        {
                            Id = r.Account.Currency.Id,
                            Abbreviation = r.Account.Currency.Abbreviation
                        }
                    },
                    FromAccount = r.FromAccount != null ? new Response.AccountResponse
                    {
                        Id = r.FromAccount.Id,
                        Name = r.FromAccount.Name,
                        Currency = new Response.AccountResponse.CurrencyResponse
                        {
                            Id = r.FromAccount.Currency.Id,
                            Abbreviation = r.FromAccount.Currency.Abbreviation
                        }
                    } : null,
                    Category = new Response.CategoryResponse
                    {
                        Id = r.Category.Id,
                        Name = r.Category.Name,
                    },
                    RecordType = r.RecordType,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (record is null)
            {
                return Errors.Record.NotFound;
            }

            return record;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/{recordId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(request.RecordId, currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
