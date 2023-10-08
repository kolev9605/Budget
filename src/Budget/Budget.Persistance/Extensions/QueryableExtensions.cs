using Budget.Domain.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Persistance.Extensions;

public static class QueryableExtensions
{
    public static async Task<IPagedListContainer<T>> PaginateAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedListContainer<T>(items, count, pageNumber, pageSize);
    }
}
