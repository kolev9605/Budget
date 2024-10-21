namespace Budget.Domain.Interfaces.Services
{
    public interface ICacheManager
    {
        Task<T> GetOrCreateAsync<T>(string key, int expirationInSeconds, Func<Task<T>> generatorAsync);
    }
}
