namespace Domain.Entities

{
    public class Lesson
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public ICollection<Block> Blocks { get; set; } = null!;
    }
}
