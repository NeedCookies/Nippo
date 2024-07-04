using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CourseRepository(AppDbContext appDbContext, IUserRepository userRepository) : ICourseRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<List<Course>> GetAllCourses()
        {
            return await _appDbContext.Courses.ToListAsync();
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

            await _appDbContext.Courses.AddAsync(course);
            await _appDbContext.SaveChangesAsync();

            return course;
        }

        public async Task<Course> Update(int id, string title, string desc, decimal price, string imgPath)
        {
            var course = await _appDbContext.Courses.FindAsync(id);

            course.Title = title;
            course.Description = desc;
            course.Price = price;
            course.ImgPath = imgPath;

            _appDbContext.Update(course);
            await _appDbContext.SaveChangesAsync();

            return course;
        }

        public async Task<Course> Delete(int courseId)
        {
            var course = await _appDbContext.Courses.FindAsync(courseId);

            if(course != null)
            {
                _appDbContext.Courses.Remove(course);
                await _appDbContext.SaveChangesAsync();
            }

            return course;
        }

        public Task<List<Course>> GetCoursesByAuthorAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Course?> GetById(int id)
        {
            return await _appDbContext.Courses.FindAsync(id);
        }

        public async Task<ApplicationUser> PurchaseCourse(int courseId, string userId)
        {
            var user = await userRepository.GetUserById(userId);
            Course course = await _appDbContext.Courses.FirstOrDefaultAsync(u => u.Id == courseId);
            decimal coursePrice = course.Price;

            if (coursePrice > user.Money)
            {
                throw new Exception("Not enough money");
            }

            user.Money -= coursePrice;

            if (user.Courses == null)
            {
                user.Courses = new List<Course>();
            }

            user.Courses.Add(course);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
    }
}
