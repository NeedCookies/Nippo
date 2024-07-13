using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserProgressRepository
    {
        Task AddedAll(List<UserProgressRequest> userProgresses);
        Task<List<UserProgress>> GetElementsByUserId(string userId);
        Task<UserProgress> UpdateProgress(UserProgressRequest userProgress);
    }
}
