using Application.Abstractions.Services;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceRegisterExt
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthServiceHttp, AuthServiceHttp>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
            services.AddScoped<IStorageService, MinIOStorageService>();
            services.AddScoped<IDiscountService, DiscountService>();

            return services;
        }
    }
}
