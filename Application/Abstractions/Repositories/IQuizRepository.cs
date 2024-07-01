using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQuizzesByCourseAsync(int courseId);
        Task<Quiz> GetQuizByIdAsync(int quizId);
        Task<Quiz> Create(int courseId, string title);
        Task<Quiz> Delete(int quizId);
    }
}
