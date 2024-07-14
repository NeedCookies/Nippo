using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuizResultRepository
    {
        Task<QuizResult?> GetQuizResultByQuizAsync(int quizId, string userId);
        Task<QuizResult> Create(int quizId, string userId, int score, int attempt);
    }
}
