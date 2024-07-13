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

        public async Task<List<UserProgress>> GetElementsByUserId(string userId)
        {
            var userProgresses = await appDbContext.UserProgresses
                .Where(up => up.UserId == userId)
                .ToListAsync();

            return userProgresses;
        }

        public async Task<UserProgress> UpdateProgress(UserProgressRequest userProgressRequest)
        {
            var userProgress = await appDbContext.UserProgresses
                .FirstOrDefaultAsync(up => up.UserId == userProgressRequest.UserId 
                    && ((userProgressRequest.ElementType == 0 && up.LessonId == userProgressRequest.ElementId)
                        || (userProgressRequest.ElementType == 1 && up.QuizId == userProgressRequest.ElementId)));

            userProgress.IsCheck = true;

            appDbContext.UserProgresses.Update(userProgress);
            await appDbContext.SaveChangesAsync();

            return userProgress;
        }
    }
}
