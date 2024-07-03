using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Add(string userName, string email, string password);
        Task<ApplicationUser> GetUserByUserName(string userName);
        Task<ApplicationUser> GetUserById(string id);
        Task<AppRole> GetDefaultUserRole();
        Task AssignRole(ApplicationUser user, string roleId);
        Task<List<Course>> GetUserCourses(string userId);
    }
}
