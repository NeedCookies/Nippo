using DataAccess;
using DataAccess.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class QuizRepositoryTest
    {
        [Fact]
        public async Task Create_NewQuiz_ReturnsCreatedQuiz()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Create_NewQuiz_ReturnsCreatedQuiz")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                // Act
                var result = await repository.Create(1, "Quiz 1", 1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Quiz 1", result.Title);
                Assert.Equal(1, result.Order);
                Assert.Equal(1, result.CourseId);
            }
        }

        [Fact]
        public async Task Delete_ExistingQuiz_ReturnsDeletedQuiz()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_ExistingQuiz_ReturnsDeletedQuiz")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var quizToDelete = new Quiz { Id = 1, Title = "Quiz To Delete", CourseId = 1, Order = 1 };
                context.Quizzes.Add(quizToDelete);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                // Act
                var result = await repository.Delete(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task Delete_NonExistingQuiz_ThrowsException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_NonExistingQuiz_ThrowsException")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                await Assert.ThrowsAsync<NullReferenceException>(() => repository.Delete(999));
            }
        }

        [Fact]
        public async Task GetQuizzesByCourseAsync_ReturnsQuizzesByCourse()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetQuizzesByCourseAsync_ReturnsQuizzesByCourse")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var quizzesData = new List<Quiz>
                {
                    new Quiz { Id = 1, Title = "Quiz 1", CourseId = 1, Order = 1 },
                    new Quiz { Id = 2, Title = "Quiz 2", CourseId = 1, Order = 2 },
                    new Quiz { Id = 3, Title = "Quiz 3", CourseId = 2, Order = 1 }
                };
                context.Quizzes.AddRange(quizzesData);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                // Act
                var result = await repository.GetQuizzesByCourseAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(q => q.CourseId == 1));
            }
        }

        [Fact]
        public async Task GetQuizByIdAsync_ExistingQuiz_ReturnsQuiz()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetQuizByIdAsync_ExistingQuiz_ReturnsQuiz")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var quizToFind = new Quiz { Id = 1, Title = "Quiz 1", CourseId = 1, Order = 1 };
                context.Quizzes.Add(quizToFind);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                var result = await repository.GetQuizByIdAsync(1);

                Assert.NotNull(result);
                Assert.Equal("Quiz 1", result.Title);
            }
        }

        [Fact]
        public async Task GetQuizByIdAsync_NonExistingQuiz_ThrowsException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetQuizByIdAsync_NonExistingQuiz_ThrowsException")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new QuizRepository(context);

                // Act & Assert
                await Assert.ThrowsAsync<NullReferenceException>(() => repository.GetQuizByIdAsync(999));
            }
        }
    }
}
