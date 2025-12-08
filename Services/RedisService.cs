
using StackExchange.Redis;
using System.Text.Json;

namespace LearningManagementSystemApi.Services
{
    public class RedisService : IRedisService
    {

        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }
        public async Task<string?> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            if (expiry.HasValue)
                await _db.StringSetAsync(key, value, expiry.Value);
            else
                await _db.StringSetAsync(key, value);
        }

        public async Task SetClassListsAsync<T>(string key, List<T> items, TimeSpan? expiry = null)
        {
            foreach (var item in items)
            {
                string json = JsonSerializer.Serialize(item);
                
                await _db.ListRightPushAsync(key, json);
            }

            if (expiry.HasValue)
            {
                await _db.KeyExpireAsync(key, expiry.Value);
            }
        }

        public async Task<List<T?>?> GetClassListAsync<T>(string key)
        {
            var values = await _db.ListRangeAsync(key);

            if(values.Length == 0)
            {
                return null;
            }

             return values
            .Select(v => JsonSerializer.Deserialize<T>(v.ToString()!))
            .ToList();
        }
    }
}
