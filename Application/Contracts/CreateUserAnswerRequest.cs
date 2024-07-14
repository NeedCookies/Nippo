namespace Application.Contracts
{
    public record CreateUserAnswerRequest (
        int QuestionId,
        string Text
        );
}
