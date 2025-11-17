namespace Shared.RedisStorage
{
    public interface IRedisStore
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetAsync(string key);
        Task DeleteAsync(string key);
    }
}
