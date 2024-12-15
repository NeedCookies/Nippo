namespace Application.Contracts
{
    public record UserProgressRequest
    (
        Guid UserId,
        int CourseId,
        int ElementId,
        int ElementType
    );
}
