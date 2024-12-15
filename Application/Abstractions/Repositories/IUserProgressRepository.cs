using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserProgressRepository
    {
        Task AddedAll(List<UserProgressRequest> userProgresses);
        Task<List<UserProgress>> GetElementsByUserCourseId(string userId, int courseId);
        Task<UserProgress> UpdateProgress(UserProgressRequest userProgress);
        Task<int> GetCompletedCourses(string userId, int courseId);
        Task<bool> GetElementStatus(UserProgressRequest userProgress);
    }
}
