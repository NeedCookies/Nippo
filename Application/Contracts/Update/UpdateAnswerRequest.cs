namespace Application.Contracts.Update
{
    public record UpdateAnswerRequest
        (
        int answerId,
        string text,
        bool isCorrect
        );
}
