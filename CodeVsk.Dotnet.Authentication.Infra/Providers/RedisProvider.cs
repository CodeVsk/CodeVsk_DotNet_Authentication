using CodeVsk.Dotnet.Authentication.Domain.Contracts.Providers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace CodeVsk.Dotnet.Authentication.Infra.Providers
{
    public class RedisProvider : IRedisProvider
    {
        private readonly IDistributedCache _cache;

        public RedisProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetAsync(string key, CancellationToken cancellationToken)
        {
            string cacheValue = await _cache.GetStringAsync(key, cancellationToken);

            if (cacheValue.IsNullOrEmpty())
            {
                return null;
            }

            return cacheValue;
        }

        public async Task<string> SetAsync(string key, string value, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(key, value, cancellationToken);

            string cacheValue = await GetAsync(key, cancellationToken);

            return cacheValue;
        }
    }
}
