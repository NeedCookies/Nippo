namespace Application.Contracts
{
    public record CreateQuizRequest(
        int CourseId,
        string Title
        );
}