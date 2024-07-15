namespace Application.Contracts.Update
{
    public record UpdateQuestionRequest
        (
        int QuestionId, 
        string Text
        );
}
