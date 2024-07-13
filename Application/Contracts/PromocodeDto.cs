namespace Application.Contracts
{
    public class PromocodeDto
    {
        public string Code { get; set; } = null!;
        public PromocodeType PromocodeType { get; set; }
        public int Discount { get; set; }
        public int ExpirationInHours { get; set; }
    }

    public enum PromocodeType
    {
        Fixed = 0,
        Percent = 1
    }
}
