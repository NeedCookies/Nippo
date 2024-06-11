using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionsByQuizAsync(int quizId);
    }
}
