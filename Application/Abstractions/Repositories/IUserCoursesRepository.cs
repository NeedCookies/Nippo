using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserCoursesRepository
    {
        Task<List<int>> GetUserCourses(string userId);
        Task<List<int>> GetAcquiredUsers(int courseId);
        Task<UserCourses> Add(int courseId, string userId);
        Task<UserCourses> Delete(int courseId, string userId);

    }
}
