using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<TResult>> BaseGetAllAsync<TResult>();

        Task<TResult> BaseGetByIdAsync<TResult>(int id);

        Task<TResult> CreateAsync<TResult>(T entity, bool saveChanges = true);

        Task<TResult> UpdateAsync<TResult>(T entity, bool saveChanges = true);

        Task<TResult> DeleteByIdAsync<TResult>(int id, bool saveChanges = true);

        Task<TResult> DeleteAsync<TResult>(T entity, bool saveChanges = true);

        Task<int> SaveChangesAsync();
    }
}
