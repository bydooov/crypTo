namespace CrypTo.Infrastructure.Services.Cache
{
    public interface ICacheService
    {
        void Set(string key, object value);
        object Get(string key);
        int GetCacheCount();
    }
}
