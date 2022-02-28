using System;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface ICacheManager
    {
        Task<T> GetOrCreateAsync<T>(string key, int expirationInMinutes, Func<Task<T>> generatorAsync);
    }
}
