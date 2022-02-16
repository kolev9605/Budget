using Budget.Core.Entities;
using Budget.Core.Entities.Base;
using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Budget.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BudgetDbContext _budgetDbContext;

        public Repository(BudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<IEnumerable<T>> AllAsync()
            => await _budgetDbContext.Set<T>().ToListAsync();

        public async Task<T> CreateAsync(T entity)
        {
            await _budgetDbContext.AddAsync(entity);

            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await _budgetDbContext.Set<T>().FindAsync(id);
            _budgetDbContext.Set<T>().Remove(entity);

            await _budgetDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
            => await _budgetDbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<T> UpdateAsync(T entity)
        {
            _budgetDbContext.Set<T>().Update(entity);
            await _budgetDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
