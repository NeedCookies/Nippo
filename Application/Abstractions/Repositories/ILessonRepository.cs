using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetLessonsByCourseAsync(int courseId);
        Task<Lesson> Create(string title, int courseId, DateTime date);
        Task<Lesson> Update(string title, int lessonId);
        Task<Lesson> Delete (int lessonId);
        Task<Lesson> GetById(int lessonId);
        Task<int> GetLessonsSizeByCourse(int courseId);
    }
}
