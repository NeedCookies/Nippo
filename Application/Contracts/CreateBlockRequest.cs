using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts
{
    public record CreateBlockRequest
    (
        int LessonId,
        int Type,
        string? Content,
        IFormFile? Media
    );
}
