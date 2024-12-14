using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);
        Task<Lesson> Create(string title, int courseId, Guid authorId, DateTime date);
        Task<Lesson> GetById(int courseId, int lessonId);
    }
}
