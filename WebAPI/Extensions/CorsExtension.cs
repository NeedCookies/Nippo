namespace WebAPI.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsWithFrontendPolicy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(name: "Proxy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return serviceCollection;
        }
    }
}
