namespace Application.Contracts
{
    public record CreateCourseRequest
        (
        string Title,
        string Description,
        decimal Price,
        string ImgPath
        );
}
