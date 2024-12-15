namespace WebAPI.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CacheSettings:RedisCache"];
            });

            return services;
        }
    }
}
