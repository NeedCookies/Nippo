using Application.Contracts;

namespace Application.Abstractions.Services
{
    public interface IUserProgressService
    {
        Task<bool> GetCourseElementStatus(UserProgressRequest userProgress);
        Task<CompletedElementsCourseResponse> GetCourseProgress(GetCompletedElementsRequest request);
    }
}
