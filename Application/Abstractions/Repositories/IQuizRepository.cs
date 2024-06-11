using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQizzesByCourseAsync(int courseId);
    }
}
