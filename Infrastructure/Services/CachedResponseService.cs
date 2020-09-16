using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class CachedResponseService : ICachedResponseService
    {
        private readonly IDatabase _database;
        public CachedResponseService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CachedResponseAsync(string cachedKey, object response, TimeSpan timeToLive)
        {
            if (response is null)
            {
                return;
            }
            var options = new JsonSerializerOptions 
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(cachedKey, json, timeToLive);
        }

        public async Task<string> GetCachedResponseAsync(string cachedKey)
        {
            var cachedResponse = await  _database.StringGetAsync(cachedKey);
            if(cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        }
    }
}