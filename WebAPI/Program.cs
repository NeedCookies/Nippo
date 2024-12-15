using DataAccess.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using WebAPI.Extensions;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using WebAPI.Middlewares;
using Serilog;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<MinIoOptions>(builder.Configuration.GetSection("Minio"));

string authServ = builder.Configuration["AuthServiceUrl"];
builder.Services.AddHttpClient("authServ",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri(authServ);
    });

var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddAppRepositories();

builder.Services.AddInfrastructureServices();
builder.Services.AddRedisCache(builder.Configuration);

builder.Services.AddAppServices();

builder.Services.AddApiAuthentication(jwtOptions);

builder.Services.AddCorsWithFrontendPolicy();

var app = builder.Build();

//В случае если приложение запускается в первый раз, и база данных не создана - будут выполнены миграции
/*
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
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
