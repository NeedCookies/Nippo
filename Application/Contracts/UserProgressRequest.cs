namespace Application.Contracts
{
    public record UserProgressRequest
    (
        string UserId,
        int ElementId,
        int ElementType
    );
}
