using Application.Abstractions.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public class PermissionAuthorizationHandler(
        IServiceScopeFactory scopeFactory) :
        AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Check if user has all required permissions. Get user permissions from auth service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(
                claim => claim.Type == CustomClaims.UserId);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ArgumentException("User Id cann't be parsed as Guid");
            }

            using var scope = scopeFactory.CreateScope();

            var authService = scope.ServiceProvider.GetRequiredService<IAuthServiceHttp>();
            var permissions = await authService.GetUserPermissionsAsync(userId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
