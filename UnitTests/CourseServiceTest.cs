using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Operations;
using Application.Services;
using DataAccess;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace UnitTests
{
    public class CourseServiceTest
    {
        private readonly Mock<ICourseRepository> _mockCourseRepository;
        private readonly Mock<IUserCoursesRepository> _mockUserCoursesRepository;
        private readonly Mock<IUserProgressRepository> _mockUserProgressRepository;
        private readonly Mock<ILessonRepository> _mockLessonRepository;
        private readonly Mock<IQuizRepository> _mockQuizRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IBasketRepository> _mockBasketRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStorageService> _mockStorageService;
        private readonly Mock<IDiscountService> _mockDiscountService;
        private readonly Mock<ILogger<CoursesService>> _mockLogger;

        private readonly ICoursesService _courseService;

        private byte[] file;
        private IFormFile imgPath;
        public CourseServiceTest()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockUserCoursesRepository = new Mock<IUserCoursesRepository>();
            _mockUserProgressRepository = new Mock<IUserProgressRepository>();
            _mockLessonRepository = new Mock<ILessonRepository>();
            _mockQuizRepository = new Mock<IQuizRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockBasketRepository = new Mock<IBasketRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockStorageService = new Mock<IStorageService>();
            _mockDiscountService = new Mock<IDiscountService>();
            _mockLogger = new Mock<ILogger<CoursesService>>();

            _courseService = new CoursesService(_mockCourseRepository.Object,
                _mockUserCoursesRepository.Object,
                _mockUserProgressRepository.Object,
                _mockLessonRepository.Object,
                _mockQuizRepository.Object,
                _mockUserService.Object,
                _mockBasketRepository.Object,
                _mockUserRepository.Object,
                _mockUnitOfWork.Object,
                _mockStorageService.Object,
                _mockDiscountService.Object,
                _mockLogger.Object);

            _mockStorageService.Setup(s => s.PutAsync(It.IsAny<string>(),
                It.IsAny<Stream>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _mockStorageService.Setup(s => s.GetUrlAsync(It.IsAny<string>())).
                ReturnsAsync("Test file url");

            file = Encoding.UTF8.GetBytes("This is a test file");
            imgPath = new FormFile(new MemoryStream(file), 0, file.Length, "Data", "test.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedCourse()
        {
            var request = new CreateCourseRequest(
                "Test course",
                "Test description",
                100,
                imgPath
                );
            var expectedCourse = new Course()
            {
                Title = "Test course",
                Description = "Test description",
                Price = 100
            };
            _mockCourseRepository.Setup(repo => repo
            .Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(),
                It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(expectedCourse); ;
            // Act
            var result = await _courseService.Create(request, "asdf-432d");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCourse.Title, result.Title);
            Assert.Equal(expectedCourse.Description, result.Description);
            Assert.Equal(expectedCourse.Price, result.Price);
        }

        [Fact]
        public async Task Create_InvalidRequest_ThrowsError()
        {
            // Arrange
            var request = new CreateCourseRequest(
                "",
                "",
                -100,
                imgPath
                );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.Create(request, "fds5-2jk4"));
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsUpdatedCourse()
        {
            // Arrange
            var courseForUpdate = new Course()
            {
                Id = 1,
                Title = "Course for update",
                Description = "Description for update",
                Price = 100,
                ImgPath = "img to update",
                AuthorId = "fd45-g5lj"
            };
            var updateRequest = new UpdateCourseRequest(
                1, "Updated title", "Updated description", 150, imgPath);
            var updatedCourse = new Course()
            {
                Title = updateRequest.Title,
                Description = updateRequest.Description,
                Price = updateRequest.Price
            };
            _mockCourseRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(courseForUpdate);
            _mockCourseRepository.Setup(repo => repo.Update(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()))
                .ReturnsAsync(updatedCourse);

            // Act
            var result = await _courseService.Update(updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateRequest.Title, result.Title);
            Assert.Equal(updateRequest.Description, result.Description);
            Assert.Equal(updateRequest.Price, result.Price);
        }

        [Fact]
        public async Task Update_InvalidRequest_ThrowsError()
        {
            var courseForUpdate = new Course()
            {
                Id = 1,
                Title = "Course for update",
                Description = "Description for update",
                Price = 100,
                ImgPath = "img to update",
                AuthorId = "fd45-g5lj"
            };
            var updateRequest = new UpdateCourseRequest(
                1, "", "", -150, imgPath);

            _mockCourseRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(courseForUpdate);

            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.Update(updateRequest));
        }

        [Fact]
        public async Task GetAllCourses_ReturnsAllCourses()
        {
            // Arrange
            var courses = new List<Course>()
            {
                new Course { Id = 1, Title = "Course 1", Description = "Description 1", Price = 120, AuthorId = "dsf34" },
                new Course { Id = 2, Title = "Course 2", Description = "Description 2", Price = 123, AuthorId = "fds4s" },
                new Course { Id = 3, Title = "Course 3", Description = "Description 3", Price = 125, AuthorId = "fds4s" },
            };
            _mockCourseRepository.Setup(repo => repo.GetAllCourses())
                .ReturnsAsync(courses);

            // Act
            var result = await _courseService.GetAllCourses();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsDeletedCourse()
        {
            // Arrange
            var courseForDelete = new Course()
            {
                Id = 1,
                Title = "Course for update",
                Description = "Description for update",
                Price = 100,
                ImgPath = "img to update",
                AuthorId = "fd45-g5lj"
            };
            _mockCourseRepository.Setup(repo => repo.Delete(courseForDelete.Id))
                .ReturnsAsync(courseForDelete);

            // Act
            var result = await _courseService.Delete(courseForDelete.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courseForDelete.Title, result.Title);
            Assert.Equal(courseForDelete.Description, result.Description);
            Assert.Equal(courseForDelete.Price, result.Price);
        }

        [Fact]
        public async Task Delete_InvalidId_ThrowsError()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.Delete(-1));
        }

        [Fact]
        public async Task GetAuthorById_ValidCourseId_ReturnsAuthorId()
        {
            // Arrange
            var courseId = 1;
            var authorId = "fg32";
            var course = new Course()
            {
                Id = 1,
                Title = "Test",
                Description = "Description",
                Price = 100,
                AuthorId = authorId
            };
            _mockCourseRepository.Setup(repo => repo.GetAuthorById(courseId))
                .ReturnsAsync(authorId);

            // Act
            var result = await _courseService.GetAuthorById(courseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result);
        }

        [Fact]
        public async Task GetAuthorById_InvalidAuthorId_ThrowsError()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.GetAuthorById(-1));
        }

        [Fact]
        public async Task GetCoursesToCheck_ReturnsCoursesToCheck()
        {
            // Arrange
            var courses = new List<Course>()
            {
                new Course { Id = 1,AuthorId="gd34", Status = 1 },
                new Course { Id = 1,AuthorId="af43", Status = 1 },
                new Course { Id = 1,AuthorId="gd34", Status = 2 },
            };
            _mockCourseRepository.Setup(repo => repo.GetCoursesByStatus(PublishStatus.Check))
                .ReturnsAsync(courses.Where(c => c.Status == 1).ToList());

            // Act
            var result = await _courseService.GetCoursesToCheck();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AcceptCourse_ReturnsModeratedCourseInfo()
        {
            var author = new ApplicationUser { Id = "sad1", UserName = "TestAuthor", Email = "test@example.com" };
            var acceptedCourse = new Course()
            {
                Id = 1,
                Title = "test",
                Description = "description",
                Price = 100,
                Status = 2,
                Author = author,
                AuthorId = author.Id,
            };
            _mockCourseRepository.Setup(repo => repo.ChangeStatus(1, PublishStatus.Publish))
                .ReturnsAsync(acceptedCourse);

            var result = await _courseService.AcceptCourse(1);

            Assert.NotNull(result);
            Assert.Equal(author.Email, result.AuthorEmail);
        }

        [Fact]
        public async Task CancelCourse_ReturnsCourseModerationInfo()
        {
            var author = new ApplicationUser { Id = "sad1", UserName = "TestAuthor", Email = "test@example.com" };
            var acceptedCourse = new Course()
            {
                Id = 1,
                Title = "test",
                Description = "description",
                Price = 100,
                Status = 2,
                Author = author,
                AuthorId = author.Id,
            };
            _mockCourseRepository.Setup(repo => repo.ChangeStatus(1, PublishStatus.Edit))
                .ReturnsAsync(acceptedCourse);

            var result = await _courseService.CancelCourse(1);

            Assert.NotNull(result);
            Assert.Equal(author.Email, result.AuthorEmail);
        }

        [Fact]
        public async Task SubmitForReview_ValidAuthorId_ReturnsSubmittedCourse()
        {
            // Arrange
            var authorId = "fds2";
            var userId = "fds2";
            var course = new Course()
            {
                Id = 1,
                AuthorId = authorId,
            };
            _mockCourseRepository.Setup(repo => repo.GetAuthorById(1))
                .ReturnsAsync(course.AuthorId);
            _mockUserService.Setup(s => s.GetUserInfoById(userId))
                .ReturnsAsync(new PersonalInfoDto() { Role = "author" });
            _mockCourseRepository.Setup(repo => repo.ChangeStatus(1, PublishStatus.Check))
                .ReturnsAsync(course);

            // Act
            var result = await _courseService.SubmitForReview(course.Id, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.AuthorId);
        }

        [Fact]
        public async Task SubmitForReview_InValidAuthor_ThrowsError()
        {
            // Arrange
            var authorId = "fds2";
            var userId = "gfd3";
            var course = new Course()
            {
                Id = 1,
                AuthorId = authorId,
            };
            _mockCourseRepository.Setup(repo => repo.GetAuthorById(1))
                .ReturnsAsync(course.AuthorId);
            _mockUserService.Setup(s => s.GetUserInfoById(userId))
                .ReturnsAsync(new PersonalInfoDto() { Role = "user" });
            _mockCourseRepository.Setup(repo => repo.ChangeStatus(1, PublishStatus.Check))
                .ReturnsAsync(course);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.SubmitForReview(course.Id, userId));
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsCourseById()
        {
            // Arrange
            var course = new Course()
            {
                Id = 1,
                Title = "Test",
                ImgPath = "Test file"
            };
            _mockCourseRepository.Setup(repo => repo.GetById(1))
                .ReturnsAsync(course);

            // Act
            var result = _courseService.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task ApplyPromocode_ValidRequest_ReturnsNewPrice()
        {
            // Arrange
            int discountSize = 30; // absolute value
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1,
                Promocode = "Test"
            };
            _mockCourseRepository.Setup(repo => repo.GetById(1))
                .ReturnsAsync(course);
            _mockDiscountService.Setup(ds => ds.ApplyPromocode(course, "Test"))
                .ReturnsAsync(int.Parse(course.Price.ToString()) - discountSize);

            // Act
            var result = await _courseService.ApplyPromocode(request);

            // Assert
            Assert.Equal(70, result);
        }

        [Fact]
        public async Task ApplyPromocode_InvalidRequest_ThrowsError()
        {
            // Arrange
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1,
                Promocode = ""
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _courseService.ApplyPromocode(request));
        }

        [Fact]
        public async Task PurchaseCourse_ValidRequest_ReturnsUserCourses()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "fdse",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1
            };
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var lessons = new List<Lesson>()
            {
                new Lesson {Id = 1, CourseId = 1},
                new Lesson {Id = 2, CourseId = 1}
            };
            var quizzes = new List<Quiz>()
            {
                new Quiz {Id = 1, CourseId = 1 },
                new Quiz {Id = 2, CourseId = 1 }
            };
            var progressRequest = new List<UserProgressRequest>()
            {
                new UserProgressRequest
                (
                    user.Id,
                    course.Id,
                    lessons[0].Id,
                    0
                )
            };
            var userCourses = new UserCourses()
            {
                Id = 1,
                CourseId = course.Id,
                UserId = user.Id,
            };
            _mockUserCoursesRepository.Setup(repo => repo.GetUserCourse(course.Id, user.Id))
                .ReturnsAsync((UserCourses)null);
            _mockUserRepository.Setup(repo => repo.GetByUserId(user.Id))
                .ReturnsAsync(user);
            _mockCourseRepository.Setup(repo => repo.GetById(course.Id))
                .ReturnsAsync(course);
            _mockBasketRepository.Setup(repo => repo.DeleteFromBasket(course.Id, user.Id));
            _mockUnitOfWork.Setup(unit => unit.SaveChangesAsync());
            _mockLessonRepository.Setup(repo => repo.GetLessonsByCourseAsync(course.Id))
                .ReturnsAsync(lessons);
            _mockQuizRepository.Setup(repo => repo.GetQuizzesByCourseAsync(course.Id))
                .ReturnsAsync(quizzes);
            _mockUserProgressRepository.Setup(repo => repo.AddedAll(progressRequest));
            _mockUserCoursesRepository.Setup(repo => repo.Add(course.Id, user.Id))
                .ReturnsAsync(userCourses);

            // Act
            var result = await _courseService.PurchaseCourse(request, user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userCourses.CourseId, result.CourseId);
            Assert.Equal(userCourses.UserId, result.UserId);
        }

        [Fact]
        public async Task PurchaseCourse_InvalidRequest_ThrowsError()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = -1
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.PurchaseCourse(request, user.Id));
        }

        [Fact]
        public async Task PurchaseCourse_CourseAlreadyBought_ThrowsError()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "fdse",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1
            };
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var userCourses = new UserCourses()
            {
                Id = 1,
                CourseId = course.Id,
                UserId = user.Id,
            };
            _mockUserCoursesRepository.Setup(repo => repo.GetUserCourse(course.Id, user.Id))
                .ReturnsAsync(userCourses);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _courseService.PurchaseCourse(request, user.Id));
        }

        [Fact]
        public async Task AddToBasket_ValidData_ReturnsBasketCourses()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "fdse",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1
            };
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var basketCourses = new BasketCourses()
            {
                Id = 1,
                CourseId = course.Id,
                UserId = user.Id
            };
            _mockBasketRepository.Setup(repo => repo.GetBasketCourse(course.Id, user.Id))
                .ReturnsAsync((BasketCourses)null);
            _mockUserCoursesRepository.Setup(repo => repo.GetUserCourse(course.Id, user.Id))
                .ReturnsAsync((UserCourses)null);
            _mockBasketRepository.Setup(repo => repo.AddtoBasket(course.Id, user.Id))
                .ReturnsAsync(basketCourses);

            // Act
            var result = await _courseService.AddToBasket(course.Id, user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course.Id, result.CourseId);
            Assert.Equal(user.Id, result.UserId);
        }

        [Fact]
        public async Task AddToBasket_AlreadyBought_ThrowsError()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "fdse",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1
            };
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var userCourses = new UserCourses()
            {
                Id = 1,
                CourseId = course.Id,
                UserId = user.Id,
            };
            _mockBasketRepository.Setup(repo => repo.GetBasketCourse(course.Id, user.Id))
                .ReturnsAsync((BasketCourses)null);
            _mockUserCoursesRepository.Setup(repo => repo.GetUserCourse(course.Id, user.Id))
                .ReturnsAsync(userCourses);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _courseService.AddToBasket(course.Id, user.Id));
        }

        [Fact]
        public async Task AddToBasket_AlreadyInBasket_ThrowsError()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Id = "fdse",
                Points = 150,
            };
            var request = new CoursePurchaseRequest()
            {
                CourseId = 1
            };
            var course = new Course()
            {
                Id = 1,
                Title = "Test Course",
                Price = 100
            };
            var basketCourses = new BasketCourses()
            {
                Id = 1,
                CourseId = course.Id,
                UserId = user.Id
            };
            _mockBasketRepository.Setup(repo => repo.GetBasketCourse(course.Id, user.Id))
                .ReturnsAsync(basketCourses);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _courseService.AddToBasket(course.Id, user.Id));
        }

        [Fact]
        public async Task DeleteFromBasket_ValidId_Return_DeletedCourse()
        {
            // Arrange
            var userId = "4nk23";
            var course = new Course()
            {
                Id = 1,
                Title = "For delete"
            };
            var deletedFromBasket = new BasketCourses()
            {
                CourseId = course.Id,
                UserId = userId
            };
            _mockBasketRepository.Setup(repo => repo.DeleteFromBasket(course.Id, userId))
                .ReturnsAsync(deletedFromBasket);

            // Act
            var result = await _courseService.DeleteFromBasket(course.Id, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(course.Id, result.CourseId);
        }

        [Fact]
        public async Task DeleteFromBasket_InvalidId_ThrowsError()
        {
            // Arrange
            var userId = "";
            var course = new Course()
            {
                Id = -1,
                Title = "For delete"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.DeleteFromBasket(course.Id, userId));
        }

        [Fact]
        public async Task GetBasketCourses_ValidUserId_ReturnsBasketCourses()
        {
            // Arrange
            var userId = "4k336";
            var basketCourses = new List<BasketCourses>()
            {
                new BasketCourses { CourseId = 1, UserId = userId},
                new BasketCourses { CourseId = 2, UserId = userId},
                new BasketCourses { CourseId = 1, UserId = "gvrei"},
            };
            _mockBasketRepository.Setup(repo => repo.GetBasketCourses(userId))
                .ReturnsAsync(basketCourses.Where(bc => bc.UserId == userId).ToList());

            // Act
            var result = await _courseService.GetBasketCourses(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetBasketCourses_InvalidUserId_ThrowsError()
        {
            // Arrange
            var userId = "";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _courseService.GetBasketCourses(userId));
        }
    }
}
