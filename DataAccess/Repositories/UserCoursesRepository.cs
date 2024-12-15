using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserCoursesRepository(AppDbContext appDbContext) : IUserCoursesRepository
    {
        public async Task<UserCourses> Add(int courseId, Guid userId)
        {
            var userCourse = new UserCourses
            {
                CourseId = courseId,
                UserId = userId
            };

            await appDbContext.UserCourses.AddAsync(userCourse);
            await appDbContext.SaveChangesAsync();

            return userCourse;
        }

        public async Task<UserCourses> Delete(int courseId, Guid userId)
        {
            var deletedUserCourse = await appDbContext.UserCourses
                .FirstOrDefaultAsync(
                uc => uc.CourseId == courseId &&
                uc.UserId == userId);

            appDbContext.UserCourses.Remove(deletedUserCourse);
            await appDbContext.SaveChangesAsync();

            return deletedUserCourse;
        }

        public async Task<List<int>> GetAcquiredUsers(int courseId)
        {
            List<int> usersId = new List<int>();

            var acquiredUsers = await appDbContext.UserCourses
                .Where(uc => uc.CourseId == courseId)
                .ToListAsync();

            foreach (var acquiredUser in acquiredUsers)
                usersId.Add(acquiredUser.CourseId);

            return usersId;
        }

        public async Task<UserCourses?> GetUserCourse(int courseId, Guid userId)
        {
            return await appDbContext.UserCourses
                .Where(uc => uc.CourseId == courseId &&
                uc.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetUserCourses(Guid userId)
        {
            List<int> coursesId = new List<int>();

            var userCourses = await appDbContext.UserCourses
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            foreach (var userCourse in userCourses)
                coursesId.Add(userCourse.CourseId);

            return coursesId;
        }

        public async Task<bool> IsCoursePurchased(string userId, int courseId)
        {
            bool isPurchased = await appDbContext.UserCourses
                .AnyAsync(uc =>
                uc.UserId == userId &&
                uc.CourseId == courseId);

            return isPurchased;
        }
    }
}
