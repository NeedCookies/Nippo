using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity user);
        Task<UserEntity> GetByEmailAsync(string email);
        Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId);
    }
}
