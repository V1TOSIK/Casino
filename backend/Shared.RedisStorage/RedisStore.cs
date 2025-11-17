using StackExchange.Redis;

namespace Shared.RedisStorage
{

    public class RedisStore : IRedisStore
    {
        private readonly IDatabase _db;

        public RedisStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
            => await _db.StringSetAsync(key, value, expiry);

        public async Task<string?> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task DeleteAsync(string key) => await _db.KeyDeleteAsync(key);
    }

}
