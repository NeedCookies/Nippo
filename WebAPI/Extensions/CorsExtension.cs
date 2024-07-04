namespace WebAPI.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsWithFrontendPolicy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(name: "Frontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return serviceCollection;
        }
    }
}
