using AuthorizationService.Application.Abstractions;
using AuthorizationService.Core;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(UserEntity user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            // Get roles as List of collection of roles
            var roles = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.Id == userId)
                .Select(u => u.Roles)
                .ToListAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (Permission)p.Id)
                .ToHashSet();
        }
        
        public async Task<Role> GetUserRoleAsync(Guid userId)
        {
            var roles = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .Select(u => u.Roles)
                .ToListAsync();

            return roles
                .SelectMany(r => r)
                .Select(r => (Role)r.Id)
                .FirstOrDefault();
        }
    }
}
