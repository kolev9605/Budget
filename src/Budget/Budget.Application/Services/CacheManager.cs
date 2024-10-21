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
            var cacheEntry = await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationInSeconds);
                return await generatorAsync();
            });

            // Note: cacheEntry shouldn't be null, but Microsoft left GetOrCreateAsync to return nullable.
            // We should handle that case and if does happen, there is nothing we can do but throw an ArgumentNullException
            // https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/api-guidelines/nullability.md
            ArgumentNullException.ThrowIfNull(cacheEntry);

            return cacheEntry;
        }
    }
}
