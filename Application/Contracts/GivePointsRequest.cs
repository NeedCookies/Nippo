namespace Application.Contracts
{
    public class GivePointsRequest
    {
        public string UserId { get; set; } = null!;
        public int Points { get; set; }
    }
}
