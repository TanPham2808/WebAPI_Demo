using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI_Demo.Rediscache.IServices;

namespace WebAPI_Demo.Rediscache
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService(IDatabase cacheDb)
        {
            _cacheDb = cacheDb;
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public object Remove(string key)
        {
            var existKey = _cacheDb.KeyExists(key);
            if(existKey)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirTime);
        }
    }
}
