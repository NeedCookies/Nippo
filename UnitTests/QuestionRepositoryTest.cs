using DataAccess;
using DataAccess.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTests
{
    public class QuestionRepositoryTest
    {
        [Fact]
        public async Task GetQuestionByQuizAsync_ReturnsQuestionsByQuiz()
        {
            var quizId = 12;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetQuestionsByQuizAsync_ReturnsQuestionsByQuiz")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var questions = new List<Question>()
                    {
                        new Question {Id = 1, Order = 1, QuizId = quizId, Text="Test", Type = QuestionType.Written},
                        new Question { Id = 2, Order = 2, QuizId = quizId, Text = "Test 2", Type = QuestionType.Written },
                        new Question {Id = 3, Order = 1, QuizId = 22, Text="Test 3", Type = QuestionType.Written}
                    };

                context.Questions.AddRange(questions);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuestionRepository(context);

                var result = await repository.GetByQuiz(quizId);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.True(result.All(q => q.QuizId == quizId));
            }
        }

        [Fact]
        public async Task GetQuestionById_ReturnsQuestionById()
        {
            var questionId = 2;
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_ReturnsQuestionById")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var questions = new List<Question>()
                    {
                        new Question {Id = 1, Order = 1, QuizId = 1, Text="Test", Type = QuestionType.Written},
                        new Question {Id = questionId, Order = 1, QuizId = 2, Text = "Test 2", Type = QuestionType.Written },
                        new Question {Id = 3, Order = 1, QuizId = 3, Text="Test 3", Type = QuestionType.Written}
                    };

                context.Questions.AddRange(questions);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuestionRepository(context);

                var result = await repository.GetById(questionId);

                Assert.NotNull(result);
                Assert.Equal(result.Text, "Test 2");
            }
        }

        [Fact]
        public async Task Create_NewQuestion_ReturnsCreatedQuestion()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Create_NewQuestion_ReturnsCreatedQuestion")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new QuestionRepository(context);

                var newQuestion = new Question()
                {
                    Text = "New Question",
                    Order = 1,
                    QuizId = 1,
                    Type = QuestionType.MultipleChoice
                };

                var result = await repository.Create(
                    newQuestion.Order,
                    newQuestion.QuizId,
                    newQuestion.Text,
                    newQuestion.Type
                    );

                Assert.NotNull(result);
                Assert.Equal(result.Order, newQuestion.Order);
                Assert.Equal(result.QuizId, newQuestion.QuizId);
                Assert.Equal(result.Text, newQuestion.Text);
                Assert.Equal(result.Type, newQuestion.Type);
            }
        }

        [Fact]
        public async Task Update_ExistingQuestion_ReturnsUpdatedQuestion()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Update_ExistingQuestion_ReturnsUpdatedQuestion")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var questionToUpdate = new Question()
                {
                    Id = 1,
                    Text = "New Question",
                    Order = 1,
                    QuizId = 1,
                    Type = QuestionType.MultipleChoice
                };

                context.Questions.Add(questionToUpdate);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuestionRepository(context);

                var updatedQuestion = new Question
                {
                    Id = 1,
                    Text = "Updated Question"
                };

                var result = await repository.Update(
                    updatedQuestion.Id,
                    updatedQuestion.Text);

                Assert.NotNull(result);
                Assert.Equal("Updated Question", result.Text);
            }
        }

        [Fact]
        public async Task Delete_ExistingQuestion_ReturnDeletedQuestion()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Delete_ExistingQuestion_ReturnDeletedQuestion")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var questionToDelete = new Question()
                {
                    Id = 1,
                    Text = "Question to delete",
                    Order = 1,
                    QuizId = 1,
                };
                context.Questions.Add(questionToDelete);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new QuestionRepository(context);

                var result = await repository.Delete(1);

                Assert.NotNull(result);
                Assert.Equal(result.Text, "Question to delete");
            }
        }
    }
}
