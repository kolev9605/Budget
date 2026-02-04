using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Models.Pagination;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetAllRecordsEndpoint : IEndpoint
{
    public class Request
    {
        public Guid? AccountId { get; set; }
        public RecordType? RecordType { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? StartDateRange { get; set; }
        public DateTime? EndDateRange { get; set; }
        public int PageNumber { get; set; } = 1;
        public int? PageSize { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public Guid? FromAccountId { get; set; }
        public string? FromAccountName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public RecordType RecordType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }

    public record Query(
        Guid? AccountId,
        RecordType? RecordType,
        Guid? CategoryId,
        DateTime? StartDateRange,
        DateTime? EndDateRange,
        int PageNumber,
        int PageSize,
        string UserId) : IRequest<ErrorOr<IPagedListContainer<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IPagedListContainer<Response>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IPagedListContainer<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var baseQuery = _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Include(r => r.FromAccount)
                .Where(r => r.Account.UserId == query.UserId);

            if (query.AccountId.HasValue)
            {
                baseQuery = baseQuery.Where(r => r.AccountId == query.AccountId.Value);
            }

            if (query.RecordType.HasValue)
            {
                baseQuery = baseQuery.Where(r => r.RecordType == query.RecordType.Value);
            }

            if (query.CategoryId.HasValue)
            {
                baseQuery = baseQuery.Where(r => r.CategoryId == query.CategoryId.Value);
            }

            if (query.StartDateRange.HasValue && query.EndDateRange.HasValue)
            {
                baseQuery = baseQuery.Where(r =>
                    r.RecordDate >= query.StartDateRange.Value &&
                    r.RecordDate <= query.EndDateRange.Value);
            }

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var records = await baseQuery
                .OrderByDescending(r => r.RecordDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(r => new Response
                {
                    Id = r.Id,
                    Note = r.Note,
                    RecordDate = r.RecordDate,
                    Amount = r.Amount,
                    AccountId = r.AccountId,
                    AccountName = r.Account.Name,
                    FromAccountId = r.FromAccountId,
                    FromAccountName = r.FromAccount != null ? r.FromAccount.Name : null,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category.Name,
                    RecordType = r.RecordType,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn
                })
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);
            var hasNextPage = query.PageNumber < totalPages;
            var hasPreviousPage = query.PageNumber > 1;

            var result = new PagedListContainer<Response>(
                records,
                query.PageNumber,
                totalPages,
                hasPreviousPage,
                hasNextPage);

            return result;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(
                    request.AccountId,
                    request.RecordType,
                    request.CategoryId,
                    request.StartDateRange,
                    request.EndDateRange,
                    request.PageNumber,
                    request.PageSize ?? PaginationConstants.DefaultPageSize,
                    currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }

    private class PagedListContainer<T> : IPagedListContainer<T>
    {
        public PagedListContainer(
            IEnumerable<T> items,
            int pageNumber,
            int totalPages,
            bool hasPreviousPage,
            bool hasNextPage)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            HasPreviousPage = hasPreviousPage;
            HasNextPage = hasNextPage;
        }

        public IEnumerable<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }
}
