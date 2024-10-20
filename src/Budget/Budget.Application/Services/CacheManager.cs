using Budget.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Budget.Application.Services
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, int expirationInSeconds, Func<Task<T>> generatorAsync)
        {
            var cacheEntry = await _memoryCache.GetOrCreateAsync<T>(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationInSeconds);
                return await generatorAsync();
            });

            return cacheEntry!;
        }
    }
}
