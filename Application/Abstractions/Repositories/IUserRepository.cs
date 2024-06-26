using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Add(string userName, string email, string password);
        Task<ApplicationUser> GetByUserName(string userName);
    }
}
