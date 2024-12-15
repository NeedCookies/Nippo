using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class LessonRepository(AppDbContext appDbContext) : ILessonRepository
    {
        public async Task<Lesson> Create(string title, int courseId, DateTime date)
        {
            var lesson = new Lesson
            {
                Title = title,
                CourseId = courseId,
                CreateDate = date
            };

            await appDbContext.Lessons.AddAsync(lesson);
            await appDbContext.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson> Update(string title, int lessonId)
        {
            var lesson = await GetById(lessonId);

            lesson.Title = title;

            appDbContext.Lessons.Update(lesson);
            await appDbContext.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson> Delete(int lessonId)
        {
            var lesson = await GetById(lessonId);

            appDbContext.Lessons.Remove(lesson);
            await appDbContext.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson> GetById(int lessonId)
        {
            return await appDbContext.Lessons.Where(
                l => l.Id == lessonId)
                .FirstAsync();
        }

        public async Task<List<Lesson>> GetLessonsByCourseAsync(int courseId)
        {
            return await appDbContext.Lessons.Where(l => l.CourseId == courseId).ToListAsync();
        }

        public async Task<int> GetLessonsSizeByCourse(int courseId) =>
            await appDbContext.Lessons
                .Where(l => l.CourseId == courseId)
                .CountAsync();
    }
}
