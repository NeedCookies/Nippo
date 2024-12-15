using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface ILessonsService
    {
        Task<List<Lesson>> GetByCourseId(int courseId, string userId);
        Task<Lesson> GetById(int lessonId, string userId);
        Task<Lesson> Create(CreateLessonRequest request);
        Task<Lesson> Update(int lessonId, string title);
        Task<Lesson> Delete(int lessonId);
    }
}
