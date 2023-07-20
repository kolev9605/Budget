using Budget.Domain.Entities.Base;
using Budget.Domain.Interfaces.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Persistance.Repositories
{
    public class Repository<T> : IRepository<T>
            where T : class, IBaseEntity
    {
        protected readonly BudgetDbContext _budgetDbContext;

        public Repository(BudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        protected IQueryable<T> GetAll()
            => _budgetDbContext.Set<T>();

        public async Task<IEnumerable<TResult>> BaseGetAllAsync<TResult>()
            => await GetAll().ProjectToType<TResult>().ToListAsync();

        public async Task<TResult> CreateAsync<TResult>(T entity, bool saveChanges = true)
        {
            var createdEntity = await _budgetDbContext.AddAsync(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return createdEntity.Entity.Adapt<TResult>();
        }

        public async Task<TResult> DeleteByIdAsync<TResult>(int id, bool saveChanges = true)
        {
            var entity = await _budgetDbContext.Set<T>().FindAsync(id);
            return await DeleteAsync<TResult>(entity, saveChanges);
        }

        public async Task<TResult> DeleteAsync<TResult>(T entity, bool saveChanges = true)
        {
            var removedEntity = _budgetDbContext.Set<T>().Remove(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return removedEntity.Adapt<TResult>();
        }

        public async Task<TResult> BaseGetByIdAsync<TResult>(int id)
            => (await _budgetDbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id)).Adapt<TResult>();

        public async Task<TResult> UpdateAsync<TResult>(T entity, bool saveChanges = true)
        {
            var updatedEntity = _budgetDbContext.Set<T>().Update(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return updatedEntity.Entity.Adapt<TResult>();
        }

        public async Task<int> SaveChangesAsync()
            => await _budgetDbContext.SaveChangesAsync();
    }
}
