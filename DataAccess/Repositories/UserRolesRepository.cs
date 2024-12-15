using Application.Abstractions.Repositories;
using Application.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRolesRepository(AppDbContext appDbContext) : IUserRolesRepository
    {
        public async Task AssignRole(string userId, string roleId)
        {
            var userRole = new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = roleId
            };

            appDbContext.UserRoles.Add(userRole);
            await appDbContext.SaveChangesAsync();
        }

        public async Task RemoveRole(string userId)
        {
            var userRole = await appDbContext.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId);
            appDbContext.UserRoles.Remove(userRole);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<IdentityUserRole<string>> GetByUserId(string userId)
        {
            var userRole = await appDbContext.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId);

            return userRole;
        }

        public async Task<List<IdentityUserRole<string>>> GetUsersAndRoles()
        {
            return await appDbContext.UserRoles.ToListAsync();
        }
    }
}
