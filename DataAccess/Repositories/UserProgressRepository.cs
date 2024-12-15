using Application.Abstractions.Repositories;
using Application.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserProgressRepository(AppDbContext appDbContext) : IUserProgressRepository
    {
        public async Task AddedAll(List<UserProgressRequest> userProgresses)
        {
            List<UserProgress> userProgressList = new List<UserProgress>();

            foreach(var userProgress in userProgresses)
            {
                UserProgress newUserProgress = new UserProgress
                {
                    UserId = userProgress.UserId,
                    CourseId = userProgress.CourseId,
                    IsCheck = false
                };

                if (userProgress.ElementType == 0)
                    newUserProgress.LessonId = userProgress.ElementId;
                else
                    newUserProgress.QuizId = userProgress.ElementId;

                userProgressList.Add(newUserProgress);
            }

            await appDbContext.UserProgresses.AddRangeAsync(userProgressList);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<UserProgress>> GetElementsByUserCourseId(Guid userId, int courseId)
        {
            var userProgresses = await appDbContext.UserProgresses
                .Where(up => up.UserId == userId 
                    && up.CourseId == courseId)
                .ToListAsync();

            return userProgresses;
        }

        public async Task<UserProgress> UpdateProgress(UserProgressRequest userProgressRequest)
        {
            var userProgress = await appDbContext.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userProgressRequest.UserId 
                    && up.CourseId == userProgressRequest.CourseId
                    && (
                        (userProgressRequest.ElementType == 0 
                        && up.LessonId == userProgressRequest.ElementId)
                        || 
                        (userProgressRequest.ElementType == 1 
                        && up.QuizId == userProgressRequest.ElementId))
                       );

            userProgress.IsCheck = true;

            appDbContext.UserProgresses.Update(userProgress);
            await appDbContext.SaveChangesAsync();

            return userProgress;
        }

        public async Task<int> GetCompletedCourses(Guid userId, int courseId) =>
            await appDbContext.UserProgresses
                .Where(up => up.UserId == userId
                    && up.CourseId == courseId 
                    && up.IsCheck == true)
                .CountAsync();

        public async Task<bool> GetElementStatus(UserProgressRequest userProgress) =>
            await appDbContext.UserProgresses
                .Where(
                    up => up.UserId == userProgress.UserId
                    && up.CourseId == userProgress.CourseId
                    && (
                        (userProgress.ElementType == 0 && up.LessonId == userProgress.ElementId)
                        || (userProgress.ElementType == 1 && up.QuizId == userProgress.ElementId)))
                .Select(up => up.IsCheck)
                .FirstOrDefaultAsync();
    }
}
