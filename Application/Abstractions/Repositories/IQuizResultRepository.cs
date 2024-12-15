using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuizResultRepository
    {
        Task<QuizResult?> GetQuizResultByQuizAsync(int quizId, Guid userId);
        Task<QuizResult> Create(int quizId, Guid userId, int score, int attempt);
    }
}
