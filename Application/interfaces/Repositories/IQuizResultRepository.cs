using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface IQuizResultRepository
    {
        Task<QuizResult> GetQuizResultByQuizAsync(int quizId);
    }
}
