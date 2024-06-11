using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAnswersByQuestion(int questionId);
    }
}
