using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Services
{
    public class CoursesService(ICourseRepository courseRepository) : ICoursesService
    {
        public async Task<Course> Create(CreateCourseRequest request, string authorId)
        {
            string title = request.Title;
            string descript = request.Description;
            decimal price = request.Price;
            string imgPath = request.ImgPath;

            StringBuilder error = new StringBuilder("");
            if (title.Length == 0)
            {
                error.Append("Title shouldn't be null");
            }
            if (descript.Length == 0)
            {
                error.Append("Description shouldn't be null");
            }
            if (price < 0)
            {
                error.Append("Prize should be equal 0 or more");
            }

            if (error.Length > 0)
            {
                throw new ArgumentException(error.ToString());
            }

            return await courseRepository.Create(title, descript, price, imgPath, authorId);
        }

        public async Task<List<Course>> GetAllCourses()
        {
            var allCourses = courseRepository.GetAllCourses();

            return await allCourses;
        }

        public async Task<Course> GetById(int id)
        {
            var course = await courseRepository.GetById(id);

            if (course == null)
            {
                throw new Exception("Course with such id was not found");
            }

            return course;
        }

        public async Task<ApplicationUser> PurchaseCourse(int courseId, string userId)
        {
            return await courseRepository.PurchaseCourse(courseId, userId);
        }
    }
}
