using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}
