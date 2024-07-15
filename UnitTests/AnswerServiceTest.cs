using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Application.Services;
using Domain.Entities;
using Moq;

namespace UnitTests
{
    public class AnswerServiceTest
    {
        private readonly Mock<IAnswerRepository> _mockAnswerRepository;
        private readonly IAnswerService _answerService;

        public AnswerServiceTest()
        {
            _mockAnswerRepository = new Mock<IAnswerRepository>();
            _answerService = new AnswerService(_mockAnswerRepository.Object);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedAnswer()
        {
            // Arrange
            var request = new CreateAnswerRequest(1, "Correct Answer", true );
            var expectedAnswer = new Answer { Id = 1, QuestionId = 1, Text = "Correct Answer", IsCorrect = true };
            _mockAnswerRepository.Setup(repo => repo.Create(request.QuestionId, request.Text, request.IsCorrect))
                .ReturnsAsync(expectedAnswer);

            // Act
            var result = await _answerService.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnswer.Id, result.Id);
            Assert.Equal(expectedAnswer.Text, result.Text);
            Assert.Equal(expectedAnswer.IsCorrect, result.IsCorrect);
        }

        [Fact]
        public async Task Create_InvalidRequest_ThrowsArgumentException()
        {
            // Arrange
            var request = new CreateAnswerRequest (-1, "", true );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _answerService.Create(request));
        }

        [Fact]
        public async Task Delete_ValidAnswerId_ReturnsDeletedAnswer()
        {
            // Arrange
            int answerId = 1;
            var expectedAnswer = new Answer { Id = 1, QuestionId = 1, Text = "Deleted Answer", IsCorrect = false };
            _mockAnswerRepository.Setup(repo => repo.Delete(answerId))
                .ReturnsAsync(expectedAnswer);

            var result = await _answerService.Delete(answerId);

            Assert.NotNull(result);
            Assert.Equal(expectedAnswer.Id, result.Id);
        }

        [Fact]
        public async Task Delete_InvalidAnswerId_ThrowsArgumentException()
        {
            int answerId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _answerService.Delete(answerId));
        }

        [Fact]
        public async Task GetById_ValidAnswerId_ReturnsAnswer()
        {
            int answerId = 1;
            var expectedAnswer = new Answer { Id = 1, QuestionId = 1, Text = "Answer 1", IsCorrect = true };
            _mockAnswerRepository.Setup(repo => repo.GetById(answerId))
                .ReturnsAsync(expectedAnswer);

            var result = await _answerService.GetById(answerId);

            Assert.NotNull(result);
            Assert.Equal(expectedAnswer.Id, result.Id);
        }

        [Fact]
        public async Task GetById_InvalidAnswerId_ThrowsArgumentException()
        {
            int answerId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _answerService.GetById(answerId));
        }

        [Fact]
        public async Task GetByQuestion_ValidQuestionId_ReturnsAnswers()
        {
            int questionId = 1;
            var expectedAnswers = new List<Answer>
            {
                new Answer { Id = 1, QuestionId = 1, Text = "Answer 1", IsCorrect = true },
                new Answer { Id = 2, QuestionId = 1, Text = "Answer 2", IsCorrect = false }
            };
            _mockAnswerRepository.Setup(repo => repo.GetByQuestion(questionId))
                .ReturnsAsync(expectedAnswers);

            var result = await _answerService.GetByQuestion(questionId);

            Assert.NotNull(result);
            Assert.Equal(expectedAnswers.Count, result.Count);
        }

        [Fact]
        public async Task GetByQuestion_InvalidQuestionId_ThrowsArgumentException()
        {
            int questionId = -1;

            await Assert.ThrowsAsync<ArgumentException>(() => _answerService.GetByQuestion(questionId));
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsUpdatedAnswer()
        {
            var request = new UpdateAnswerRequest ( 1, "Updated Answer", true );
            var expectedAnswer = new Answer { Id = 1, QuestionId = 1, Text = "Updated Answer", IsCorrect = true };
            _mockAnswerRepository.Setup(repo => repo.Update(request.answerId, request.text, request.isCorrect))
                .ReturnsAsync(expectedAnswer);

            var result = await _answerService.Update(request);

            Assert.NotNull(result);
            Assert.Equal(expectedAnswer.Id, result.Id);
            Assert.Equal(expectedAnswer.Text, result.Text);
            Assert.Equal(expectedAnswer.IsCorrect, result.IsCorrect);
        }

        [Fact]
        public async Task Update_InvalidRequest_ThrowsArgumentException()
        {
            var request = new UpdateAnswerRequest (-1, "", true );

            await Assert.ThrowsAsync<ArgumentException>(() => _answerService.Update(request));
        }
    }
}
