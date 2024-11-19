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
    }
}
