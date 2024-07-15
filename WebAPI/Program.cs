using DataAccess.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using Infrastructure.Options;
using WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<MinIoOptions>(builder.Configuration.GetSection("Minio"));

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddAppRepositories();
builder.Services.AddInfrastructureServices();
builder.Services.AddIdentityServices();
builder.Services.AddAppServices();

builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy();

var app = builder.Build();

//В случае если приложение запускается в первый раз, и база данных не создана - будут выполнены миграции
await using (var scope = app.Services.CreateAsyncScope())
{
    await Task.Delay(1000);

    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    //var userService = scope.ServiceProvider.GetRequiredService<UserService>();

    if (dbContext!.Database.IsRelational())
    {
        await dbContext.Database.MigrateAsync();
        //await userService.Register("admin", "admin@mail.ru", "admin");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseMiddleware<AuthorizationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
