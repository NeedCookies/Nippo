using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CourseRepository(
        AppDbContext appDbContext,
        IUserRepository userRepository,
        IUserCoursesRepository userCoursesRepository) : ICourseRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Course>> GetAllCourses() =>
            await _appDbContext.Courses.AsNoTracking()
                .Where(c => c.Status == (int)PublishStatus.Publish)
                .ToListAsync();

        public async Task<Course> Create(string title, string desc, decimal price, string imgPath, Guid authorId)
        {
            var course = new Course
            {
                Title = title,
                Description = desc,
                Price = price,
                AuthorId = authorId,
                ImgPath = imgPath,
                Status = 0
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

            if (course == null)
            {
                throw new NullReferenceException($"No course with id {courseId}");
            }

            _appDbContext.Courses.Remove(course);
            await _appDbContext.SaveChangesAsync();

            return course;
        }

        public Task<List<Course>> GetCoursesByAuthorAsync(Guid authorId) =>
            await _appDbContext.Courses
            .Where(c => c.AuthorId == authorId)
            .ToListAsync();

        public async Task<Course?> GetById(int id) =>
            await _appDbContext.Courses
            .FindAsync(id);

        public async Task<string> GetAuthorById(int id)
        {
            var course = await _appDbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new NullReferenceException($"No course with id: {id}");

            return course.AuthorId;
        }


        public async Task<List<Course>> GetCoursesByStatus(PublishStatus status) =>
            await _appDbContext.Courses.Where(c => c.Status == (int)status).ToListAsync();

        public async Task<Course> ChangeStatus(int courseId, PublishStatus status)
        {
            var course = await _appDbContext.Courses.Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == courseId);

        public async Task<ApplicationUser> PurchaseCourse(int courseId, Guid userId)
        {
            var user = new ApplicationUser();
            //await userRepository.GetByUserId(userId);
            Course course = await _appDbContext.Courses.FirstOrDefaultAsync(u => u.Id == courseId);
            decimal coursePrice = course.Price;
            if (course == null)
                return null;

            course.Status = (int)status;

            _appDbContext.Courses.Update(course);
            await _appDbContext.SaveChangesAsync();

            return course;
        }
    }
}
