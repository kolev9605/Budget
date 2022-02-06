namespace Budget.Core.Interfaces.Repositories
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, int>
    {
    }
}
