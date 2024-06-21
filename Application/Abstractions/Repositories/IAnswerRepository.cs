using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAnswersByQuestion(int questionId);
    }
}
