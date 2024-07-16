using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Operations;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Services
{
    public class CoursesService(
        ICourseRepository courseRepository, 
        IUserCoursesRepository userCoursesRepository,
        IUserProgressRepository userProgressRepository,
        ILessonRepository lessonRepository,
        IQuizRepository quizRepository,
        IUserService userService,
        IBasketRepository basketRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        IDiscountService discountService,
        ILogger<CoursesService> logger) : ICoursesService
    {
        public async Task<Course> Create(CreateCourseRequest request, string authorId)
        {
            string title = request.Title;
            string descript = request.Description;
            decimal price = request.Price;

            if (request.ImgPath == null)
            {
                throw new ArgumentNullException("Bad logo type");
            }

            var logoStream = request.ImgPath.OpenReadStream();
            var fileName = Guid.NewGuid().ToString();

            await storageService.PutAsync(fileName, logoStream, request.ImgPath.ContentType);

            string imgPath = fileName;

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
            var course = await courseRepository.GetById(request.Id);

            if (course == null)
            {
                throw new ArgumentException($"No course with Id{request.Id}");
            }

            StringBuilder error = new StringBuilder("");
            if (request.Title.Length == 0)
            {
                error.Append("Title shouldn't be null");
            }
            if (request.Description.Length == 0)
            {
                error.Append("Description shouldn't be null");
            }
            if (request.Price < 0)
            {
                error.Append("Prize should be equal 0 or more");
            }
            if (request.ImgPath == null)
            {
                error.Append("Bad logo type");
            }

            if (error.Length > 0)
            {
                throw new ArgumentException(error.ToString());
            }

            course.Title = request.Title;
            course.Description = request.Description;
            course.Price = request.Price;

            var logoStream = request.ImgPath.OpenReadStream();
            var fileName = Guid.NewGuid().ToString();

            await storageService.PutAsync(fileName, logoStream, request.ImgPath.ContentType);

            course.ImgPath = fileName;

            await unitOfWork.SaveChangesAsync();

            return await courseRepository.GetById(request.Id);
        }

        public async Task<List<Course>> GetAllCourses()
        {
            var allCourses = await courseRepository.GetAllCourses();

            foreach (var course in allCourses)
            {
                course.ImgPath = course.ImgPath != null ?
                    await storageService.GetUrlAsync(course.ImgPath) :
                    null;
            }

            return allCourses;
        }

        public async Task<Course> Delete(int courseId) => 
            await courseRepository.Delete(courseId);

        public async Task<string> GetAuthorById(int id) =>
            await courseRepository.GetAuthorById(id);

        public async Task<List<Course>> GetCoursesToCheck()
        {
            var allCourses = await courseRepository.GetCoursesByStatus(PublishStatus.Check);

            foreach (var course in allCourses)
            {
                course.ImgPath = course.ImgPath != null ?
                    await storageService.GetUrlAsync(course.ImgPath) :
                    null;
            }

            return allCourses;
        }

        public async Task<ModeratedCourseInfo> AcceptCourse(int courseId)
        {
            logger.LogInformation("Course was accepted. Course Id: {courseId}", courseId);
            var course = await courseRepository.ChangeStatus(courseId, PublishStatus.Publish);

            return new ModeratedCourseInfo()
            {
                AdminAnswer = $@"Здравствуйте, {course.Author.UserName}!
                    Рады сообщить, что ваш курс успешно прошёл модерацию и был размещён на нашем сайте. Теперь пользователи могут ознакомиться с вашим материалом и начать обучение.
                    Благодарим вас за вклад в развитие нашей платформы и желаем удачи в дальнейшей работе!

                    С уважением,
                    Администрация",

                AuthorEmail = course.Author.Email!
            };
        }
            

        public async Task<ModeratedCourseInfo> CancelCourse(int courseId)
        {
            logger.LogWarning("Course was canceled. Course Id: {courseId} ", courseId);
            var course = await courseRepository.ChangeStatus(courseId, PublishStatus.Edit);

            return new ModeratedCourseInfo()
            {
                AdminAnswer = $@"Здравствуйте, {course.Author.UserName}!
                Ваш курс был отправлен на доработку. Пожалуйста, ознакомьтесь с правилами размещения курсов на нашем сайте и внесите необходимые изменения. После этого вы можете снова отправить курс на модерацию.

                Благодарим за ваше понимание и сотрудничество.

                С уважением,
                Администрация",

                AuthorEmail = course.Author.Email!
            };
        }

        public async Task<Course> SubmitForReview(int courseId, string userId)
        {
            var courseAuthor = await courseRepository.GetAuthorById(courseId);
            var userInfo = await userService.GetUserInfoById(userId);

            logger.LogInformation("Course has been sent for review. Course Id: {CourseId}", courseId);

            if (courseAuthor == userId && userInfo.Role == "author")
                return await courseRepository.ChangeStatus(courseId, PublishStatus.Check);
            else
                throw new Exception($"Access denied courseAuthor: {courseAuthor}" +
                    $" userId: {userId} userInfo: {userInfo.Role}");
        }

        public async Task<Course> GetById(int id)
        {
            var course = await courseRepository.GetById(id);

            if (course == null)
                throw new Exception("Course with such id was not found");

            var imgPath = course.ImgPath != null ?
                await storageService.GetUrlAsync(course.ImgPath) :
                null;

            course.ImgPath = imgPath;

            return course;
        }

        public async Task<int> ApplyPromocode(CoursePurchaseRequest request)
        {
            if (request == null || request.Promocode == null)
                throw new ArgumentNullException("Bad promocode");

            var course = await courseRepository.GetById(request.CourseId);
            int newPrice = await discountService.ApplyPromocode(course, request.Promocode);

            return newPrice;
        }

        public async Task<UserCourses> PurchaseCourse(CoursePurchaseRequest request, string userId)
        {
            int courseId = request.CourseId;

            StringBuilder error = new StringBuilder();
            if (userId == null)
                error.AppendLine("userId is null");
            if (courseId < 0)
                error.AppendLine("Wrong course id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            var maybeBought = await userCoursesRepository.GetUserCourse(courseId, userId);

            if (maybeBought != null)
                throw new InvalidOperationException(
                    $"Course already bought by user: {maybeBought.UserId}");

            var user = await userRepository.GetByUserId(userId);
            var course = await courseRepository.GetById(courseId);

            if (request.Promocode != null)
            {
                course.Price = await ApplyPromocode(request);
            }

            if (user.Points < course.Price)
                throw new ApplicationException("Don't have enough points");

            await basketRepository.DeleteFromBasket(courseId, userId);
            user.Points -= (int)course.Price;
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Course was bought by user. Course Id: {CourseId}. User Id: {UserId}");

            var courseLessons = await lessonRepository.GetLessonsByCourseAsync(courseId);
            var courseQuizzes = await quizRepository.GetQuizzesByCourseAsync(courseId);
            List<UserProgressRequest> progressRequest = new List<UserProgressRequest>();

            int courseSize = courseLessons.Count + courseQuizzes.Count;

            for(int i = 0; i < courseLessons.Count; i++)
            {
                UserProgressRequest newRequest = new UserProgressRequest
                (
                    userId,
                    courseId,
                    courseLessons[i].Id,
                    0
                );

                progressRequest.Add(newRequest);
            }

            for (int i = 0; i < courseQuizzes.Count; i++)
            {
                UserProgressRequest newRequest = new UserProgressRequest
                (
                    userId,
                    courseId,
                    courseQuizzes[i].Id,
                    1
                );

                progressRequest.Add(newRequest);
            }

            await userProgressRepository.AddedAll(progressRequest);
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
                throw new InvalidOperationException(
                    $"Course already in basket: {maybeAdded}");

            var maybeBought = await userCoursesRepository.GetUserCourse(courseId, userId);
            
            if (maybeBought != null)
                throw new InvalidOperationException(
                    $"Course already bought by user: {maybeBought}");

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
