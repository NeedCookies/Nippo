using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Services
{
    public class CoursesService(
        ICourseRepository courseRepository, 
        IUserCoursesRepository userCoursesRepository,
        IBasketRepository basketRepository) : ICoursesService
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

        public async Task<Course> Update(UpdateCourseRequest request)
        {
            int id = request.Id;
            string title = request.Title;
            string descript = request.Description;
            decimal price = request.Price;
            string imgPath = request.ImgPath;

            return await courseRepository.Update(id, title, descript, price, imgPath);
        }

        public async Task<Course> Delete(int courseId) => await courseRepository.Delete(courseId);

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

        public async Task<UserCourses> PurchaseCourse(int courseId, string userId)
        {
            StringBuilder error = new StringBuilder();
            if (userId == null)
                error.AppendLine("userId is null");
            if (courseId < 0)
                error.AppendLine("Wrong course id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            var maybeBought = await userCoursesRepository.GetUserCourse(courseId, userId);
            if (maybeBought != null)
            {
                throw new InvalidOperationException(
                    $"Course already bought by user: {maybeBought}"
                    );
            }

            await basketRepository.DeleteFromBasket(courseId, userId);
            return await userCoursesRepository.Add(courseId, userId);
        }

        public async Task<BasketCourses> AddToBasket(int courseId, string userId)
        {
            StringBuilder error = new StringBuilder();
            if (userId == null)
                error.AppendLine("userId is null");
            if (courseId < 0)
                error.AppendLine("Wrong course id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            var maybeAdded = await basketRepository.GetBasketCourse(courseId, userId);
            if (maybeAdded != null)
            {
                throw new InvalidOperationException(
                    $"Course already in basket: {maybeAdded}"
                    );
            }

            var maybeBought = await userCoursesRepository.GetUserCourse(courseId, userId);
            if (maybeBought != null)
            {
                throw new InvalidOperationException(
                    $"Course already bought by user: {maybeBought}"
                    );
            }

            return await basketRepository.AddtoBasket(courseId, userId);
        }

        public async Task<BasketCourses> DeleteFromBasket(int courseId, string userId)
        {
            StringBuilder error = new StringBuilder();
            if (userId == null)
                error.AppendLine("userId is null");
            if (courseId < 0)
                error.AppendLine("Wrong course id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            return await basketRepository.DeleteFromBasket(courseId, userId);
        }

        public async Task<List<BasketCourses>> GetBasketCourses(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException("userId is null");

            return await basketRepository.GetBasketCourses(userId);
        }
    }
}
