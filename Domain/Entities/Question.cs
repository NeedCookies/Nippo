namespace Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public int Order { get; set; }
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        Written
    }
}
