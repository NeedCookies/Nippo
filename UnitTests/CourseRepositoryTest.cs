using Application.Abstractions.Repositories;
using DataAccess;
using DataAccess.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTests
{
    public class CourseRepositoryTest
    {
        [Fact]
        public async Task GetCoursesByAuthorAsync_ReturnsCoursesByAuthor()
        {
            // Arrange
            var authorId = "author123";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetCoursesByAuthorAsync_ReturnsCoursesByAuthor")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var coursesData = new List<Course>
                {
                    new Course { Id = 1, Title = "Course 1", AuthorId = authorId },
                    new Course { Id = 2, Title = "Course 2", AuthorId = authorId },
                    new Course { Id = 3, Title = "Course 3", AuthorId = "otherAuthor" }
                };
                context.Courses.AddRange(coursesData);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetCoursesByAuthorAsync(authorId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(c => c.AuthorId == authorId));
            }
        }

        [Fact]
        public async Task GetAllCourses_ReturnsAllCourses()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllCourses_ReturnsAllCourses")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var coursesData = new List<Course>
                {
                    new Course { Id = 1, Title = "Course 1" },
                    new Course { Id = 2, Title = "Course 2" },
                    new Course { Id = 3, Title = "Course 3" }
                };
                context.Courses.AddRange(coursesData);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetAllCourses();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
            }
        }

        [Fact]
        public async Task GetCreatedCourses_ReturnsCoursesCreatedByUser()
        {
            // Arrange
            var userId = "user123";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetCreatedCourses_ReturnsCoursesCreatedByUser")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var coursesData = new List<Course>
                {
                    new Course { Id = 1, Title = "Course 1", AuthorId = userId },
                    new Course { Id = 2, Title = "Course 2", AuthorId = userId },
                    new Course { Id = 3, Title = "Course 3", AuthorId = "otherUser" }
                };
                context.Courses.AddRange(coursesData);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetCreatedCourses(userId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(c => c.AuthorId == userId));
            }
        }

        [Fact]
        public async Task Create_NewCourse_ReturnsCreatedCourse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Create_NewCourse_ReturnsCreatedCourse")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                var newCourse = new Course
                {
                    Title = "New Course",
                    Description = "Description of New Course",
                    Price = 100,
                    AuthorId = "author123",
                    ImgPath = "new_course.jpg"
                };

                // Act
                var result = await repository.Create(newCourse.Title, newCourse.Description, newCourse.Price, newCourse.ImgPath, newCourse.AuthorId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newCourse.Title, result.Title);
                Assert.Equal(newCourse.Description, result.Description);
                Assert.Equal(newCourse.Price, result.Price);
                Assert.Equal(newCourse.AuthorId, result.AuthorId);
                Assert.Equal(newCourse.ImgPath, result.ImgPath);
            }
        }

        [Fact]
        public async Task Update_ExistingCourse_ReturnsUpdatedCourse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Update_ExistingCourse_ReturnsUpdatedCourse")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var courseToUpdate = new Course
                {
                    Id = 1,
                    Title = "Original Title",
                    Description = "Original Description",
                    Price = 50,
                    ImgPath = "original_img.jpg"
                };
                context.Courses.Add(courseToUpdate);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                var updatedCourse = new Course
                {
                    Id = 1,
                    Title = "Updated Title",
                    Description = "Updated Description",
                    Price = 100,
                    ImgPath = "updated_img.jpg"
                };

                // Act
                var result = await repository.Update(updatedCourse.Id, updatedCourse.Title, updatedCourse.Description, updatedCourse.Price, updatedCourse.ImgPath);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedCourse.Title, result.Title);
                Assert.Equal(updatedCourse.Description, result.Description);
                Assert.Equal(updatedCourse.Price, result.Price);
                Assert.Equal(updatedCourse.ImgPath, result.ImgPath);
            }
        }

        [Fact]
        public async Task Update_NonExistingCourse_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Update_NonExistingCourse_ReturnsNull")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.Update(999, "Updated Title", "Updated Description", 100, "updated_img.jpg");

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task Delete_ExistingCourse_ReturnsDeletedCourse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_ExistingCourse_ReturnsDeletedCourse")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var courseToDelete = new Course {  };
                context.Courses.Add(courseToDelete);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.Delete(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task Delete_NonExistingCourse_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_NonExistingCourse_ReturnsNull")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.Delete(999);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetById_ExistingCourse_ReturnsCourse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_ExistingCourse_ReturnsCourse")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var courseToFind = new Course { Id = 1, Title = "Found Course" };
                context.Courses.Add(courseToFind);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Found Course", result.Title);
            }
        }

        [Fact]
        public async Task GetById_NonExistingCourse_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_NonExistingCourse_ReturnsNull")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetById(999);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAuthorById_ExistingCourse_ReturnsAuthorId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAuthorById_ExistingCourse_ReturnsAuthorId")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var courseToFind = new Course { Id = 1, AuthorId = "author123" };
                context.Courses.Add(courseToFind);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetAuthorById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("author123", result);
            }
        }

        [Fact]
        public async Task GetAuthorById_NonExistingCourse_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAuthorById_NonExistingCourse_ReturnsNull")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.GetAuthorById(999);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ChangeStatus_NonExistingCourse_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ChangeStatus_NonExistingCourse_ReturnsNull")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var mockUserRepository = new Mock<IUserRepository>();
                var mockUserCoursesRepository = new Mock<IUserCoursesRepository>();

                var repository = new CourseRepository(context, mockUserRepository.Object, mockUserCoursesRepository.Object);

                // Act
                var result = await repository.ChangeStatus(999, PublishStatus.Publish);

                // Assert
                Assert.Null(result);
            }
        }

    }
}