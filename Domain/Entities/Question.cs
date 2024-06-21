namespace Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public string Text { get; set; } = null!;
        public QuestionType Type { get; set; }
        public ICollection<Answer> Answers { get; set; } = null!;
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        Written
    }
}
