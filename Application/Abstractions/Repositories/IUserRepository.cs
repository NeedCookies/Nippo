using Domain.Entities;
using Domain.Entities.Identity;


namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Add(string userName, string email, string password);
        Task<ApplicationUser> GetByUserName(string userName);
        Task<ApplicationUser?> GetByUserId(string userId);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<AppRole> GetDefaultUserRole();
        Task<List<Course>> GetUserCourses(string userId);
    }
}
