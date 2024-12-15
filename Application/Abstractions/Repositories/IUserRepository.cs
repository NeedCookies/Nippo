using Domain.Entities;
using Domain.Entities.Identity;


namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Add(Guid userId,
            string firstName, string lastName,
            string userName, string email,
            DateOnly birthDate, string phoneNumber,
            AppRole role, int points
            );
        Task<ApplicationUser?> GetByUserName(string userName);
        Task<ApplicationUser?> GetByEmail(string email);
        /// <summary>
        /// Returns full user entity
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApplicationUser?> GetByUserId(Guid userId);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<AppRole> GetUserRole(Guid userId);
        Task<List<Course>> GetUserCourses(Guid userId);
    }
}
