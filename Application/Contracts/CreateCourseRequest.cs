using Microsoft.AspNetCore.Http;

namespace Application.Contracts
{
    public record CreateCourseRequest
        (
        string Title,
        string Description,
        decimal Price,
        IFormFile ImgPath
        );
}
