using Domain.Entities;

namespace Application.Contracts
{
    public record CreateQuestionRequest(
        int Order,
        int QuizId,
        string Text,
        QuestionType Type
        );
}
