namespace Application.Contracts
{
    public record UpdateCourseRequest
        (
        int Id,
        string Title,
        string Description,
        decimal Price,
        string ImgPath
        );
}
