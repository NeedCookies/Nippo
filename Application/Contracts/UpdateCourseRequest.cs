namespace Application.Contracts
{
    public record UpdateCourseRequest
        (
        int id,
        string Title,
        string Description,
        decimal Price,
        string ImgPath
        );
}
