namespace Application.Contracts.Update
{
    public record UpdateUserAnswerRequest(
        int UserAnswerId,
        string Text
        );
}
