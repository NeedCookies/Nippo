namespace Application.Contracts
{
    public record CreateQuizRequest(
        int courseId,
        string Title
        );
}