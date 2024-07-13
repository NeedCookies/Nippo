namespace Application.Contracts
{
    public record UserProgressRequest
    (
        string UserId,
        int CourseId,
        int ElementId,
        int ElementType
    );
}
