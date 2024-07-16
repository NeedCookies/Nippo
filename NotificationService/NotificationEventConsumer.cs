using Application.Contracts;
using MassTransit;
using NotificationService.Services;

namespace NotificationService
{
    public class NotificationEventConsumer (IEmailSender emailSender) : IConsumer<NotificationEvent>
    {
        public async Task Consume(ConsumeContext<NotificationEvent> context)
        {
            var notification = context.Message;

            if (notification.Type == NotificationType.Email)
            {
                await emailSender.SendEmailAsync(notification.Recipient, notification.Subject, notification.Body);
            }
        }
    }
}
