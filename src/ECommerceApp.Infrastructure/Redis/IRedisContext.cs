using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Infrastructure.Redis;

public interface IRedisContext
{
    Task<T> GetObject<T>(string key); 
    Task Remove(string key);
    Task SaveObject<T>(string key, T objectInstance, int? expireMinutes = null);
}
