using Budget.Core.Entities.Base;
using Budget.Core.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class, IBaseEntity
    {
        protected readonly BudgetDbContext _budgetDbContext;

        public Repository(BudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<IEnumerable<T>> AllAsync()
            => await _budgetDbContext.Set<T>().ToListAsync();

        public async Task<T> CreateAsync(T entity, bool saveChanges = true)
        {
            await _budgetDbContext.AddAsync(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<T> DeleteAsync(int id, bool saveChanges = true)
        {
            var entity = await _budgetDbContext.Set<T>().FindAsync(id);
            _budgetDbContext.Set<T>().Remove(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
            => await _budgetDbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<T> UpdateAsync(T entity, bool saveChanges = true)
        {
            _budgetDbContext.Set<T>().Update(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<int> SaveChangesAsync()
            => await _budgetDbContext.SaveChangesAsync();
    }
}
