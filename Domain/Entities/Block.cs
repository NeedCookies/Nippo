namespace Domain.Entities
{
    public class Block
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public BlockType Type { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
    }

    public enum BlockType
    {
        Text,
        Image,
        Video
    }
}
