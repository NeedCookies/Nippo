using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetByCourseId(int courseId);
        Task<Quiz> GetById(int quizId);
        Task<Quiz> Create(CreateQuizRequest request);
        Task<Quiz> Delete(int quizId);
    }
}
