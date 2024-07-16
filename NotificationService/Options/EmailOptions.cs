namespace NotificationService.Options
{
    public class EmailOptions
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string MailFrom { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
