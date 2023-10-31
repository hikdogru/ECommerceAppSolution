using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ECommerceApp.Infrastructure.Redis;
public class RedisContext : IRedisContext
{
    private readonly IDistributedCache _distributedCache;
    private readonly Dictionary<string, object> _cache;

    public RedisContext(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _cache = new Dictionary<string, object>();
    }

    public async Task<T> GetObject<T>(string key)
    {
        if (_cache.ContainsKey(key))
        {
            return (T)_cache[key];
        }

        var serializedObject = await _distributedCache.GetStringAsync(key);
        if (string.IsNullOrWhiteSpace(serializedObject))
        {
            return default(T);
        }
        else
        {
            var result = JsonConvert.DeserializeObject<T>(serializedObject);
            _cache.Add(key, result);
            return result;
        }
    }

    public async Task Remove(string key)
    {
        if (_cache.ContainsKey(key))
            _cache.Remove(key);

        await _distributedCache.RemoveAsync(key);
    }


    public async Task SaveObject<T>(string key, T objectInstance, int? expireMinutes = null)
    {
        var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(objectInstance);
        if (_cache.ContainsKey(key))
        {
            _cache[key] = objectInstance;
        }
        else
        {
            _cache.Add(key, objectInstance);
        }

        await _distributedCache.SetStringAsync(key, serializedObject, options: new DistributedCacheEntryOptions
        { SlidingExpiration = expireMinutes.HasValue ? TimeSpan.FromMinutes(expireMinutes.Value) : TimeSpan.MaxValue });
    }
}
