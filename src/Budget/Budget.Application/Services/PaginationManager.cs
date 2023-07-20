using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class PaginationManager : IPaginationManager
    {
        public PaginationManager()
        {
        }

        public async Task<PaginationModel<T>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginationModel<T>(items, count, pageNumber, pageSize);
        }
    }
}
