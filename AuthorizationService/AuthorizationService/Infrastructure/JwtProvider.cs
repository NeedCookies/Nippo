using AuthorizationService.Application.Abstractions;
using AuthorizationService.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationService.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options) => _options = options.Value;

        public string GenerateToken(User user)
        {
            Claim[] claims = [new ("userId", user.Id.ToString()), 
                new ("email", user.Email.ToString()),
                new ("regDate", user.RegistrationDate.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.Expires),
                signingCredentials: signingCredentials
                );

            var tokenVal = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenVal;
        }
    }
}
