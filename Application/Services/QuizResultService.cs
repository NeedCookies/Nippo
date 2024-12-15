using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;

namespace Application.Services
{
    internal class QuizResultService(
        IQuizResultRepository quizResultRepository,
        IAnswerRepository answerRepository,
        IUserAnswerRepository userAnswerRepository,
        IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork,
        IUserProgressRepository userProgressRepository,
        IQuizRepository quizRepository)
        : IQuizResultService
    {
        public async Task<QuizResult> GetQuizResult(string userId, int quizId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"Wrong user Id {userId}");
            if (quizId <= 0)
                throw new ArgumentException($"Wrong quiz Id {quizId}");

            return await quizResultRepository.GetQuizResultByQuizAsync(quizId, guidUserId);
        }

        public async Task<QuizResult> SaveQuizResult(int quizId, string userId)
        {
            int score = 0;

            var questions = await questionRepository.GetByQuiz(quizId);
            int allQuestions = questions.Count;

            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"Wrong user Id {userId}");

            foreach (var question in questions)
            {
                var answers = await answerRepository.GetRightByQuestion(question.Id);

                if (question.Type == QuestionType.Written)
                {
                    var userAnswer = await userAnswerRepository.GetByQuestion(guidUserId, question.Id);
                    if (userAnswer.Text.CompareTo(answers[0].Text) == 0)
                    {
                        score++;
                    }
                }
                else if (question.Type == QuestionType.SingleChoice)
                {
                    var userAnswer = await userAnswerRepository.GetByQuestion(guidUserId, question.Id);
                    // we store user answers for questions with multiple ans single
                    // answers in answer ids
                    if (answers[0].Id.ToString().CompareTo(userAnswer.Text) == 0)
                    {
                        score++;
                    }
                }
                else
                {
                    var userAnswer = await userAnswerRepository.GetByQuestion(guidUserId, question.Id);
                    // we store user answers for multiple answers like string of answers ids '1 2 3 ...'
                    List<int> userAnswersIds = new List<int>();
                    foreach (var answerId in userAnswer.Text.Split(' '))
                    {
                        userAnswersIds.Add(int.Parse(answerId));
                    }
                    userAnswersIds.Sort();

                    List<int> questionAnswersIds = new List<int>();
                    foreach (var answer in answers)
                    {
                        questionAnswersIds.Add(answer.Id);
                    }
                    questionAnswersIds.Sort();

                    if (questionAnswersIds.Count != userAnswersIds.Count)
                        continue;

                    for (int i = 0; i < userAnswersIds.Count; i++)
                    {
                        if (userAnswersIds[i] != questionAnswersIds[i])
                        { continue; }
                    }

                    score++;
                }
            }

            double scoreRatio = (double)score / allQuestions;
            score = (int)Math.Round(scoreRatio * 10, MidpointRounding.AwayFromZero);

            var quizResult = await quizResultRepository.GetQuizResultByQuizAsync(quizId, guidUserId);

            if (score > 5)
            {
                var quiz = await quizRepository.GetQuizByIdAsync(quizId);

                await userProgressRepository.UpdateProgress(
                    new UserProgressRequest
                    (
                        guidUserId,
                        quiz.CourseId,
                        quizId,
                        1
                    )
                );
            }

            if (quizResult == null)
            {
                int attempt = 1;
                return await quizResultRepository.Create(quizId, guidUserId, score, attempt);
            }
            else
            {
                quizResult.Attempt++;
                quizResult.Score = score;

                await unitOfWork.SaveChangesAsync();
                return quizResult;
            }
        }
    }
}
