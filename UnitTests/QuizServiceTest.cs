using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests
{
    public class QuizServiceTest
    {
        private readonly Mock<IQuizRepository> _mockQuizRepository;
        private readonly IQuizService _quizService;

        public QuizServiceTest()
        {
            _mockQuizRepository = new Mock<IQuizRepository>();
            _quizService = new QuizService(_mockQuizRepository.Object);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedQuiz()
        {
            // Arrange
            var request = new CreateQuizRequest (1,"Test Quiz" );
            var expectedQuiz = new Quiz { Id = 1, CourseId = 1, Title = "Test Quiz", Order = 1 };
            _mockQuizRepository.Setup(repo => repo.GetQuizzesByCourseAsync(request.CourseId))
                .ReturnsAsync(new List<Quiz>());
            _mockQuizRepository.Setup(repo => repo.Create(request.CourseId, request.Title, 1))
                .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _quizService.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuiz.Id, result.Id);
            Assert.Equal(expectedQuiz.CourseId, result.CourseId);
            Assert.Equal(expectedQuiz.Title, result.Title);
            Assert.Equal(expectedQuiz.Order, result.Order);
        }

        [Fact]
        public async Task Create_InvalidCourseId_ThrowsArgumentException()
        {
            // Arrange
            var request = new CreateQuizRequest ( -1, "Test Quiz" );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _quizService.Create(request));
        }

        [Fact]
        public async Task Create_EmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var request = new CreateQuizRequest ( 1, "" );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _quizService.Create(request));
        }

        [Fact]
        public async Task GetByCourseId_ValidCourseId_ReturnsQuizzes()
        {
            // Arrange
            int courseId = 1;
            var expectedQuizzes = new List<Quiz>
            {
                new Quiz { Id = 1, CourseId = 1, Title = "Quiz 1", Order = 1 },
                new Quiz { Id = 2, CourseId = 1, Title = "Quiz 2", Order = 2 }
            };
            _mockQuizRepository.Setup(repo => repo.GetQuizzesByCourseAsync(courseId))
                .ReturnsAsync(expectedQuizzes);

            // Act
            var result = await _quizService.GetByCourseId(courseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuizzes.Count, result.Count);
            Assert.Equal(expectedQuizzes[0].Id, result[0].Id);
            Assert.Equal(expectedQuizzes[1].Id, result[1].Id);
        }

        [Fact]
        public async Task GetByCourseId_InvalidCourseId_ThrowsArgumentException()
        {
            // Arrange
            int courseId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _quizService.GetByCourseId(courseId));
        }

        [Fact]
        public async Task GetById_ValidQuizId_ReturnsQuiz()
        {
            // Arrange
            int quizId = 1;
            var expectedQuiz = new Quiz { Id = 1, CourseId = 1, Title = "Quiz 1", Order = 1 };
            _mockQuizRepository.Setup(repo => repo.GetQuizByIdAsync(quizId))
                .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _quizService.GetById(quizId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuiz.Id, result.Id);
            Assert.Equal(expectedQuiz.CourseId, result.CourseId);
            Assert.Equal(expectedQuiz.Title, result.Title);
            Assert.Equal(expectedQuiz.Order, result.Order);
        }

        [Fact]
        public async Task GetById_InvalidQuizId_ThrowsArgumentException()
        {
            // Arrange
            int quizId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _quizService.GetById(quizId));
        }

        [Fact]
        public async Task Delete_ValidQuizId_ReturnsDeletedQuiz()
        {
            // Arrange
            int quizId = 1;
            var expectedQuiz = new Quiz { Id = 1, CourseId = 1, Title = "Quiz 1", Order = 1 };
            _mockQuizRepository.Setup(repo => repo.Delete(quizId))
                .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _quizService.Delete(quizId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuiz.Id, result.Id);
            Assert.Equal(expectedQuiz.CourseId, result.CourseId);
            Assert.Equal(expectedQuiz.Title, result.Title);
            Assert.Equal(expectedQuiz.Order, result.Order);
        }

        [Fact]
        public async Task Delete_InvalidQuizId_ThrowsArgumentException()
        {
            // Arrange
            int quizId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _quizService.Delete(quizId));
        }
    }
}