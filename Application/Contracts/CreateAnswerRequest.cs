namespace Application.Contracts
{
    public record CreateAnswerRequest(
        int QuestionId,
        string Text,
        bool isCorrect
        );
}
