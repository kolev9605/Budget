using Budget.Core.Models;
using Budget.Core.Models.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IPaginationManager
    {
        Task<PaginationModel<T>> CreateAsync<T>(IQueryable<T> source, int pageIndex, int pageSize);
    }
}
