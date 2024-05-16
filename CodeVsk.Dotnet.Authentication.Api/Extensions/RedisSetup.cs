using StackExchange.Redis;

namespace CodeVsk.Dotnet.Authentication.Api.Extensions
{
    public static class RedisSetup
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigurationOptions redisConfiguration = new ConfigurationOptions
            {
                EndPoints = { configuration["Redis:Address"] },
                Password = configuration["Redis:Password"],
            };

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "instance";
                options.ConfigurationOptions = redisConfiguration;
            });
        }
    }
}
