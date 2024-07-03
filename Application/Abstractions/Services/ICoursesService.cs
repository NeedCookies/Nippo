using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface ICoursesService
    {
        public Task<List<Course>> GetAllCourses();
        public Task<Course> Create(CreateCourseRequest request, string authorId);
        public Task<Course> GetById(int id);
        public Task<ApplicationUser> PurchaseCourse(int courseId, string userId);
    }
}
