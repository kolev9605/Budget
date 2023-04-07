using Budget.Application.Models;
using Budget.Application.Models.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IPaginationManager
    {
        Task<PaginationModel<T>> CreateAsync<T>(IQueryable<T> source, int pageIndex, int pageSize);
    }
}
