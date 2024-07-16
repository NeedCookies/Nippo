using DataAccess;
using DataAccess.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class AnswerRepositoryTest
    {
        [Fact]
        public async Task GetAsnwersByQuestion_ReturnsAnswers()
        {
            var questionId = 1;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAsnwersByQuestion_ReturnsAnswers")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var answers = new List<Answer>()
                {
                    new Answer {Id = 1, QuestionId = questionId, IsCorrect=true, Text="Answer 1"},
                    new Answer {Id = 2, QuestionId = 2, IsCorrect=true, Text="Answer 2"},
                    new Answer {Id = 3, QuestionId = questionId, IsCorrect=true, Text="Answer 3"},
                };

                context.Answers.AddRange(answers);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);

                var result = await repository.GetByQuestion(questionId);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(a => a.QuestionId == questionId));
            }
        }

        [Fact]
        public async Task GetRightAnswersByQuestion_ReturnRightAnswersByQuestion()
        {
            int questionId = 1;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetRightAnswersByQuestion_ReturnRightAnswersByQuestion")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var answers = new List<Answer>()
                {
                    new Answer {Id = 1, QuestionId = questionId, IsCorrect=true, Text="Answer 1"},
                    new Answer {Id = 2, QuestionId = questionId, IsCorrect=false, Text="Answer 2"},
                    new Answer {Id = 3, QuestionId = questionId, IsCorrect=true, Text="Answer 3"},
                };

                context.Answers.AddRange(answers);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);

                var result = await repository.GetRightByQuestion(questionId);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(a => a.QuestionId == questionId));
            }
        }
        [Fact]
        public async Task GetAnswerById_ReturnsAnswerById()
        {
            var questionId = 1;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAnswerById_ReturnsAnswerById")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var answer = new Answer
                {
                    Id = 1,
                    QuestionId = questionId,
                    Text = "Test answer",
                    IsCorrect = true
                };

                await context.Answers.AddAsync(answer);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);
                var result = await repository.GetById(questionId);

                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal(questionId, result.QuestionId);
                Assert.Equal("Test answer", result.Text);
            }
        }

        [Fact]
        public async Task Create_NewAnswer_ReturnsCreatedAnswer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Create_NewAnswer_ReturnsCreatedAnswer")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);

                var newAnswer = new Answer
                {
                    QuestionId = 1,
                    IsCorrect = false,
                    Text = "Test Answer"
                };

                var result = await repository.Create(
                    newAnswer.QuestionId,
                    newAnswer.Text,
                    newAnswer.IsCorrect);

                Assert.NotNull(result);
                Assert.Equal(result.IsCorrect, newAnswer.IsCorrect);
                Assert.Equal(result.Text, newAnswer.Text);
                Assert.Equal(result.QuestionId, newAnswer.QuestionId);
            }
        }

        [Fact]
        public async Task DeleteExistingAnswerById_ReturnsDeletedAnswer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteExistingAnswerById_ReturnsDeletedAnswer")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var answerToDelete = new Answer
                {
                    QuestionId = 1,
                    Text = "Answer to delete",
                    IsCorrect = true
                };
                context.Answers.Add(answerToDelete);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);

                var result = await repository.Delete(1);

                Assert.NotNull(result);
                Assert.Equal(1, result.QuestionId);
                Assert.Equal("Answer to delete", result.Text);
            }
        }

        [Fact]
        public async Task UpdateExistingAnswerById_ReturnsUpdatedAnswer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAnswerById_ReturnsUpdatedAnswer")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var answerToUpdate = new Answer()
                {
                    Id = 1,
                    QuestionId = 1,
                    Text = "Answer to update",
                    IsCorrect = true
                };

                context.Answers.Add(answerToUpdate);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new AnswerRepository(context);

                var updatedAnswer = new Answer
                { 
                    Id = 1,
                    Text = "Updated answer",
                    IsCorrect = true
                };

                var result = await repository.Update(
                    updatedAnswer.Id,
                    updatedAnswer.Text,
                    updatedAnswer.IsCorrect
                    );

                Assert.NotNull(result);
                Assert.Equal("Updated answer", result.Text);
                Assert.Equal(1, result.QuestionId);
            }
        }
    }
}
