using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);
    }
}
