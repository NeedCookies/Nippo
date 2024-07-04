using DataAccess.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

builder.Services.AddIdentity<ApplicationUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddAppRepositories();
builder.Services.AddInfrastructureServices();
builder.Services.AddIdentityServices();
builder.Services.AddAppServices();

builder.Services.AddApiAuthentication(jwtOptions);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
});

builder.Services.AddCorsWithFrontendPolicy();

var app = builder.Build();

//В случае если приложение запускается в первый раз, и база данных не создана - будут выполнены миграции
await using (var scope = app.Services.CreateAsyncScope())
{
    await Task.Delay(1000);

    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    if (dbContext!.Database.IsRelational())
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
