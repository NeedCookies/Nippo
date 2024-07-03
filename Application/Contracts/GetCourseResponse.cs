namespace Application.Contracts
{
    public record GetCourseResponse
        (
        int Id,
        string Title,
        string Description,
        decimal Price,
        string ImgPath
        );
}
