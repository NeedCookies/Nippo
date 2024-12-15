using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BasketRepostory(AppDbContext dbContext) : IBasketRepository
    {
        public async Task<BasketCourses> AddtoBasket(int courseId, Guid userId)
        {
            var basketCourse = new BasketCourses()
            {
                CourseId = courseId,
                UserId = userId
            };

            await dbContext.BasketCourses.AddAsync(basketCourse);
            await dbContext.SaveChangesAsync();

            return basketCourse;
        }

        public async Task<BasketCourses> DeleteFromBasket(int courseId, Guid userId)
        {
            var deleteFromBasket = await dbContext.BasketCourses.
                FirstOrDefaultAsync(
                bc => bc.CourseId == courseId &&
                bc.UserId == userId);

            if (deleteFromBasket == null)
            {
                throw new NullReferenceException(
                    $"There's no course in basket with id {courseId}");
            }

            dbContext.BasketCourses.Remove(deleteFromBasket);
            await dbContext.SaveChangesAsync();

            return deleteFromBasket;
        }

        public async Task<List<BasketCourses>> GetBasketCourses(Guid userId) =>
            await dbContext.BasketCourses
                .Where(bc => bc.UserId == userId)
                .ToListAsync();

        public async Task<BasketCourses?> GetBasketCourse(int courseId, Guid userId) => 
            await dbContext.BasketCourses
                .Where(bc => bc.UserId == userId
                && bc.CourseId == courseId)
                .FirstOrDefaultAsync();
    }
}
