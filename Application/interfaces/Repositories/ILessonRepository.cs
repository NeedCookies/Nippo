using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);
    }
}
