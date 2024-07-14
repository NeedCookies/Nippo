namespace Domain.Entities
{
    public class Block
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public BlockType Type { get; set; }
        public string Content { get; set; } = null!;
        public int Order { get; set; }
    }

    public enum BlockType
    {
        Text,
        Image,
        Video
    }
}
