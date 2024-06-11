using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionsByQuizAsync(int quizId);
    }
}
