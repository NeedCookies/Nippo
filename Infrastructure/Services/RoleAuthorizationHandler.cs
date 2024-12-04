using Application.Abstractions.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public class RoleAuthorizationHandler(
        IServiceScopeFactory scopeFactory) :
        AuthorizationHandler<RolesAuthorizationRequirement>
    { 
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(
                claim => claim.Type == CustomClaims.UserId);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ArgumentException("User Id cann't be parsed as Guid");
            }

            using var scope = scopeFactory.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<IAuthServiceHttp>();
            var role = await authService.GetUserRoleAsync(userId);

            if (requirement.AllowedRoles.Contains(role.Trim('\"', '\\')))
            {
                context.Succeed(requirement);
            }
        }
    }
}
