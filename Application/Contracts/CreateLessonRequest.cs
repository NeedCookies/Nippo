namespace Application.Contracts
{
    public record CreateLessonRequest
        (
        int CourseId,
        string Title,
        string AuthorId
        );
}
