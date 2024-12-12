namespace Domain.Entities
{
    public class Block
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        /// <summary>
        /// BlockType represents 3 types: text, image, video
        /// </summary>
        public BlockType Type { get; set; }
        /// <summary>
        /// Depend on 'Type' property and will
        /// contain text, or url to image or to video in S3
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
