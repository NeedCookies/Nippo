using AuthorizationService.Persistance;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddDbContext<AppDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
                });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
