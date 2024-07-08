using Domain.Entities;

namespace Application.Contracts
{
    public record CreateBlockRequest
        (
        int LessonId,
        int Type,
        string Content,
        int Order
        );
}
