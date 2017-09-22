using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SmartMirrorSchedule.Data.Services
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
    }
    public class RedisCacheService : IRedisCacheService
    {
        private static string RedisCacheConnectionString => ConfigurationManager.ConnectionStrings["RedisCacheConnectionString"].ConnectionString ?? "localhost:6379";
        private static bool UseRedisCache => Convert.ToBoolean(ConfigurationManager.AppSettings["UseRedisCache"] ?? "true");
        private static TimeSpan RedisCacheExpiration => new TimeSpan(0, 5, 0);

        private static Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(RedisCacheConnectionString);
        });

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                if (UseRedisCache)
                {
                    return await Connection.GetDatabase().GetAsync<T>(key);
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            try
            {
                if (UseRedisCache)
                {
                    await Connection.GetDatabase().SetAsync(key, value, RedisCacheExpiration);
                }
            }
            catch
            {
                return;
            }
        }
    }

    public static class RedisExtensions
    {
        public static async Task<T> GetAsync<T>(this IDatabase cache, string key)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(await cache.StringGetAsync(key));
                return result;
            }
            catch
            {
                return default(T);
            }
        }

        public static async Task SetAsync<T>(this IDatabase cache, string key, T value, TimeSpan expiration)
        {
            await cache.StringSetAsync(key, JsonConvert.SerializeObject(value), expiration);
        }
    }
}