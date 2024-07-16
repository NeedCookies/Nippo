using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Options;

namespace NotificationService.Services
{
    public class EmailSender (IOptionsMonitor<EmailOptions> emailOptions) : IEmailSender
    {
        private readonly EmailOptions _emailOptions = emailOptions.CurrentValue;

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            using var smtp = new SmtpClient();
            var mailMessage = CreateMailMessage(recipient, body, subject);

            try
            {
                await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailOptions.MailFrom, _emailOptions.Password);

                await smtp.SendAsync(mailMessage);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private MimeMessage CreateMailMessage(string emailTo, string body, string subject)
        {
            var emailMessage = new MimeMessage();

            emailMessage.Sender = MailboxAddress.Parse(_emailOptions.MailFrom);
            emailMessage.To.Add(MailboxAddress.Parse(emailTo));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            return emailMessage;
        }
    }
}
