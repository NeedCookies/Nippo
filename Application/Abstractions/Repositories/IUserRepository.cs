using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Add(string userName, string email, string password);
        Task<ApplicationUser> GetByUserName(string userName);
        Task<AppRole> GetDefaultUserRole();
        Task AssignRole(ApplicationUser user, string roleId);
    }
}
