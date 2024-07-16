namespace Application.Contracts
{
    public record GetCompletedElementsRequest
    (
        string userId,
        int courseId
    );
}
