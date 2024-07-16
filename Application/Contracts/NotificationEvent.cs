namespace Application.Contracts
{
    public class NotificationEvent
    {
        public string Recipient { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public NotificationType Type { get; set; }
    }

    public enum NotificationType
    {
        Email
    }
}
