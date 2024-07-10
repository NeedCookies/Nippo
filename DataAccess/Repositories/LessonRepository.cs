using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class LessonRepository(AppDbContext dbContext) : ILessonRepository
    {
        public async Task<Lesson> Create(string title, int courseId, DateTime date)
        {
            var lesson = new Lesson
            {
                Title = title,
                CourseId = courseId,
                CreateDate = date
            };

            await dbContext.Lessons.AddAsync(lesson);
            await dbContext.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson> GetById(int lessonId)
        {
            return await dbContext.Lessons.Where(
                l => l.Id == lessonId)
                .FirstAsync();
        }

        public async Task<List<Lesson>> GetLessonsByCourseAsync(int courseId)
        {
            return await dbContext.Lessons.Where(l => l.CourseId == courseId).ToListAsync();
        }
    }
}
