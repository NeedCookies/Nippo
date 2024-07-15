using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests
{
    public class QuestionServiceTest
    {
        private readonly Mock<IQuestionRepository> _mockQuestionRepository;
        private readonly IQuestionService _questionService;

        public QuestionServiceTest()
        {
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _questionService = new QuestionService(_mockQuestionRepository.Object);
        }

        [Fact]
        public async Task Create_ValidRequest_RetursCreatedQuestion()
        {
            // Arrange
            var request = new CreateQuestionRequest(1, "Test question", "Written");
            var expectedQuestion = new Question { 
                Id = 1, 
                Order = 1,
                Text = "Test question",
                QuizId = 1,
                Type = QuestionType.Written
            };
            // Настройка мока для метода GetByQuiz
            _mockQuestionRepository.Setup(repo => repo.GetByQuiz(request.QuizId))
                .ReturnsAsync(new List<Question>()); 

            // Настройка мока для метода Create
            _mockQuestionRepository.Setup(repo => repo.Create(1,
                request.QuizId, request.Text, QuestionType.Written))
                .ReturnsAsync(expectedQuestion);

            // Act
            var result = await _questionService.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuestion.Id, result.Id);
            Assert.Equal(expectedQuestion.QuizId, result.QuizId);
            Assert.Equal(expectedQuestion.Text, result.Text);
            Assert.Equal(expectedQuestion.Type, result.Type);
            Assert.Equal(expectedQuestion.QuizId, result.QuizId);
        }

        [Fact]
        public async Task Create_InvalidRequest_ThrowsArgumentException()
        {
            var request = new CreateQuestionRequest(-1, "", "Wrong");

            await Assert.ThrowsAsync<ArgumentException>(() => _questionService.Create(request));
        }

        [Fact]
        public async Task Delete_ValidQuestionId_ReturnsDeletedQuestion()
        {
            var questionId = 1;
            var expextedQuestion = new Question
            {
                Id = 1,
                Text = "Test",
                QuizId = 1,
                Type = QuestionType.Written
            };

            _mockQuestionRepository.Setup(repo => repo.Delete(questionId))
                .ReturnsAsync(expextedQuestion);

            var result = await _questionService.Delete(questionId);

            Assert.NotNull(result);
            Assert.Equal(expextedQuestion.Id, result.Id);
        }

        [Fact]
        public async Task Delete_InvalidQuestioId_ThrowsArgumentException()
        {
            int answerId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _questionService.Delete(answerId));
        }

        [Fact]
        public async Task GetByQuizId_ValidQuizId_ReturnsQuestions()
        {
            int quizId = 1;
            var expectedQuestions = new List<Question>
            {
                new Question { Id = 1, Text = "Test", QuizId = quizId },
                new Question { Id = 2, Text = "Test 2", QuizId = quizId }
            };
            _mockQuestionRepository.Setup(repo => repo.GetByQuiz(quizId))
                .ReturnsAsync(expectedQuestions);

            var result = await _questionService.GetByQuizId(quizId);

            Assert.NotNull(result);
            Assert.Equal(expectedQuestions.Count, result.Count);
        }

        [Fact]
        public async Task GetByQuizId_InvalidQuizId_ThrowsException()
        {
            int quizId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _questionService.GetByQuizId(quizId));
        }

        [Fact]
        public async Task GetById_ValidQuestionId_ReturnsQuestion()
        {
            int questionId = 1;
            var expectedQuestion = new Question
            {
                Id = 1
            };
            _mockQuestionRepository.Setup(repo => repo.GetById(questionId))
                .ReturnsAsync(expectedQuestion);

            var result = await _questionService.GetById(questionId);

            Assert.NotNull(result);
            Assert.Equal(expectedQuestion.Id, result.Id);
        }

        [Fact]
        public async Task GetById_InvalidQuestionId_ThrowsException()
        {
            int questionId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _questionService.GetById(questionId));
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsUpdatedAnswer()
        {
            var request = new UpdateQuestionRequest(1, "Updated Question");
            var expectedAnswer = new Question { Id = 1, Text = "Updated Question" };
            _mockQuestionRepository.Setup(repo => repo.Update(request.QuestionId, request.Text)).
                ReturnsAsync(expectedAnswer);

            var result = await _questionService.Update(request);

            Assert.NotNull(result);
            Assert.Equal(expectedAnswer.Text, result.Text);
        }

        [Fact]
        public async Task Update_InvalidRequest_ThrowsException()
        {
            var request = new UpdateQuestionRequest(-1, "");

            await Assert.ThrowsAsync<ArgumentException>(() => _questionService.Update(request));
        }
    }
}
