using Infrastructure.Entities;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Extensions;
public static class ApiExt
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IOptions<JwtOptions> jwtOptions,
        params string[] policies)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CreateCourse", policy => policy.Requirements.Add(
                new PermissionRequirement(Permission.CreateCourse)));
            options.AddPolicy("CreatePromo", policy => policy.Requirements.Add(
                new PermissionRequirement(Permission.CreatePromo)));
        });
    }
}
