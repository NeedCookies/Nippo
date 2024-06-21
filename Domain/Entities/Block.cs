namespace Domain.Entities
{
    public class Block
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public BlockType Type { get; set; }
        /// <summary>
        /// depend on typr content property will
        /// contain text, or url on image or video
        /// </summary>
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
