using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WebAPI_Demo.Rediscache.IServices;

namespace WebAPI_Demo.Rediscache
{
    public static class CacheRegistion
    {
        public static IServiceCollection AddRedisCacheService(this IServiceCollection services)
        {
            services.AddSingleton<IDatabase>(cfg =>
            {
                var redisConnection = ConnectionMultiplexer.Connect($"localhost:6379");
                return redisConnection.GetDatabase();
            });

            services.AddSingleton<ICacheService, CacheService>();

            return services;
        }
    }
}
