using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface ILessonsService
    {
        Task<List<Lesson>> GetByCourseId(int courseId);
        Task<Lesson> GetById(int courseId, int lessonId);
        Task<Lesson> Create(CreateLessonRequest request);
    }
}
