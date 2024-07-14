using Microsoft.AspNetCore.Http;

namespace Application.Contracts
{
    public record UpdateCourseRequest
        (
        int Id,
        string Title,
        string Description,
        decimal Price,
        IFormFile ImgPath
        );
}
