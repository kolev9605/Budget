using Budget.Domain.Entities;
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

        public async Task<IEnumerable<T>> BaseGetAllAsync()
            => await GetAll().ToListAsync();

        public async Task<T> CreateAsync(T entity, bool saveChanges = true)
        {
            var createdEntity = await _budgetDbContext.AddAsync(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return createdEntity.Entity;
        }

        public async Task<T> DeleteByIdAsync(int id, bool saveChanges = true)
        {
            var entity = await _budgetDbContext.Set<T>().FindAsync(id);
            return await DeleteAsync(entity, saveChanges);
        }

        public async Task<T> DeleteAsync(T entity, bool saveChanges = true)
        {
            var removedEntity = _budgetDbContext.Set<T>().Remove(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return removedEntity.Entity;
        }

        public async Task<T> BaseGetByIdAsync(int id)
            => (await _budgetDbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id));

        public async Task<T> UpdateAsync(T entity, bool saveChanges = true)
        {
            var updatedEntity = _budgetDbContext.Set<T>().Update(entity);

            if (saveChanges)
            {
                await _budgetDbContext.SaveChangesAsync();
            }

            return updatedEntity.Entity;
        }

        public async Task<int> SaveChangesAsync()
            => await _budgetDbContext.SaveChangesAsync();
    }
}
