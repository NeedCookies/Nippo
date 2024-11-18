using AuthorizationService.Application.Abstractions;
using AuthorizationService.Application.Services;
using AuthorizationService.Endpoints;
using AuthorizationService.Infrastructure;
using AuthorizationService.Persistance;
using AuthorizationService.Persistance.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();

            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

            services.AddDbContext<AppDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
                });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthorizationService API V1");
                });
            }

            app.UseRouting();
            app.MapAuthEndpoints();

            app.Run();
        }
    }
}
