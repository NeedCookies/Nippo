using DataAccess;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Extensions
{
    public static class UserManagerRegistrationExt
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
