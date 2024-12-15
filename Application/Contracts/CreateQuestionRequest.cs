using Domain.Entities;

namespace Application.Contracts
{
    public record CreateQuestionRequest(
        int QuizId,
        string Text,
        string Type
        );
}
