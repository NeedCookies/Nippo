namespace Application.Contracts.Operations
{
    public record CoursePurchaseRequest
    {
        public int CourseId { get; set; }
        public string? Promocode { get; set; }
    }
}
