using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Repositories
{
    public class CourseRepository(AppDbContext appDbContext, IUserRepository userRepository) : ICourseRepository
    {
        public async Task<List<Course>> GetAllCourses()
        {
            return await appDbContext.Courses.ToListAsync();
        }

        public async Task<Course> Create(string title, string desc, decimal price, string imgPath, string authorId)
        {
            var course = new Course
            {
                Title = title,
                Description = desc,
                Price = price,
                AuthorId = authorId,
                ImgPath = imgPath
            };

            await appDbContext.Courses.AddAsync(course);
            await appDbContext.SaveChangesAsync();

            return course;
        }
        public Task<List<Course>> GetCoursesByAuthorAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Course?> GetById(int id)
        {
            return await appDbContext.Courses.FindAsync(id);
        }

        public async Task<ApplicationUser> PurchaseCourse(int courseId, string userId)
        {
            var user = await userRepository.GetUserById(userId);
            Course course = await appDbContext.Courses.FirstOrDefaultAsync(u => u.Id == courseId);
            decimal coursePrice = course.Price;

            if (coursePrice > user.Money)
            {
                throw new Exception("Not enough money");
            }

            user.Money -= coursePrice;

            user.Courses.Add(course);
            await appDbContext.SaveChangesAsync();

            return user;
        }
    }
}
