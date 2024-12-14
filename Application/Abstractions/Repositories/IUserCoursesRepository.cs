using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    /// <summary>
    /// Represents relationship between users and their bought courses
    /// </summary>
    public interface IUserCoursesRepository
    {
        /// <summary>
        /// Returns list if course ids which was bought by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetUserCourses(Guid userId);
        /// <summary>
        /// Returns list of user ids who was bought this course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<List<int>> GetAcquiredUsers(int courseId);
        Task<UserCourses> Add(int courseId, Guid userId);
        Task<UserCourses> Delete(int courseId, Guid userId);

    }
}
