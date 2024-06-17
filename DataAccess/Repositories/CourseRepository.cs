using Application.Abstractions.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CourseRepository(AppDbContext dbContext) : ICourseRepository
    {
        public async Task<List<Course>> GetAllCourses()
        {
            return await dbContext.Courses.ToListAsync();
        }

        public async Task<Course> Create(string title, string desc, decimal price, string imgPath)
        {
            var course = new Course();
            course.Title = title;
            course.Description = desc;
            course.Price = price;
            course.ImgPath = imgPath;

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return course;
        }
        public Task<List<Course>> GetCoursesByAuthorAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Course> GetById(int id)
        {
            return await dbContext.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
