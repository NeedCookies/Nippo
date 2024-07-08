using Application.Abstractions.Services;
using Domain.Entities.Identity;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtProvider(IOptions<JwtOptions> options,
        UserManager<ApplicationUser> userManager) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public async Task<string> GenerateAsync(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            //Claim[] claims = [new("userId", user.Id), new(ClaimTypes.Role, userRoles.First())];

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id)
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
