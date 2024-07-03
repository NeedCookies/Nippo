using DataAccess;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Extensions
{
    public static class UserManagerRegistrationExt
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>(options => { })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<RoleManager<AppRole>>();
            return services;
        }
    }
}
