namespace Budget.Domain.Interfaces.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> BaseGetAllAsync();

    Task<T?> BaseGetByIdAsync(Guid id);

    Task<T> CreateAsync(T entity, bool saveChanges = true);

    Task<T> UpdateAsync(T entity, bool saveChanges = true);

    Task<T> DeleteByIdAsync(Guid id, bool saveChanges = true);

    Task<T> DeleteAsync(T entity, bool saveChanges = true);

    Task<int> SaveChangesAsync();
}
