using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserProgressRepository
    {
        Task AddedAll(List<UserProgressRequest> userProgresses);
        Task<List<UserProgress>> GetElementsByUserCourseId(Guid userId, int courseId);
        Task<UserProgress> UpdateProgress(UserProgressRequest userProgress);
        Task<int> GetCompletedCourses(Guid userId, int courseId);
        Task<bool> GetElementStatus(UserProgressRequest userProgress);
    }
}
