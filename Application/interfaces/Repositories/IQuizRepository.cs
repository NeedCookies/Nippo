using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetQizzesByCourseAsync(int courseId);
    }
}
