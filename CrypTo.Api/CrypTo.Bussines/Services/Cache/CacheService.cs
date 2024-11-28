using CrypTo.Infrastructure.Services.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace CrypTo.Bussines.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly MemoryCache _cache;

        public CacheService()
        {
            _cache = new(new MemoryCacheOptions { });
        }

        public void Set(string key, object value)
        {
            _cache.Set(key, value);
        }

        public int GetCacheCount()
        {
            return _cache.Count;
        }

        public object Get(string key)
        {
            var result = _cache.Get(key);
            _cache.Remove(key);

            return result!;
        }
    }
}
