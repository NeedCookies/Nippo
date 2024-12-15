using Application.Abstractions.Repositories;
using Domain.Entities.Identity;

namespace DataAccess.Repositories
{
    public class UserRolesRepository(
        AppDbContext appDbContext, IUserRepository userRepository
        ) : IUserRolesRepository
    {
        public async Task AssignRole(Guid userId, AppRole role)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
                throw new NullReferenceException($"There's no user with id: {userId}");
            
            user.Role = role;
            await appDbContext.SaveChangesAsync();
        }

        public async Task SetDefaultRole(Guid userId)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
                throw new NullReferenceException($"There's no user with id: {userId}");

            user.Role = AppRole.User;
            await appDbContext.SaveChangesAsync();
        }

        public async Task<AppRole> GetByUserId(Guid userId)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
                throw new NullReferenceException($"There's no user with id: {userId}");

            return user.Role;
        }
    }
}
