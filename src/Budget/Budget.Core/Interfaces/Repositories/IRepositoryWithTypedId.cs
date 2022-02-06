namespace Budget.Core.Interfaces.Repositories
{
    public interface IRepositoryWithTypedId<T, IdT>
    {
        T Get(IdT id);

        IList<T> GetAll();

        IList<T> FindAll(IDictionary<string, object> propertyValuePairs);

        T FindOne(IDictionary<string, object> propertyValuePairs);

        T SaveOrUpdate(T entity);

        void Delete(T entity);

        IBudgetDbContext DbContext { get; }
    }
}
