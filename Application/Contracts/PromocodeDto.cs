namespace Application.Contracts
{
    public class PromocodeDto
    {
        public string Code { get; set; } = null!;
        public PromocodeType PromocodeType { get; set; }
        public int Discount { get; set; }
        public int ExpirationInHours { get; set; }

        public PromocodeDto(string code,
        PromocodeType promocodeType,
        int discount,
        int expirationInHours)
        {
            Code = code;
            PromocodeType = promocodeType;
            Discount = discount;
            ExpirationInHours = expirationInHours;
        }
    }

    public enum PromocodeType
    {
        Fixed = 0,
        Percent = 1
    }
}
