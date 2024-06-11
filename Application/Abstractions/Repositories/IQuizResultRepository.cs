using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuizResultRepository
    {
        Task<QuizResult> GetQuizResultByQuizAsync(int quizId);
    }
}
