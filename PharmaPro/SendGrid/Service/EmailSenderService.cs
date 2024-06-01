using Microsoft.Extensions.Options;
using PharmaPro.SendGrid.Model;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace PharmaPro.SendGrid.Service
{
    public class EmailSenderService
    {
        public interface IEmailSender
        {
            Task SendEmailAsync(string toEmail, string subject, string message);
        }

        public class SendGridEmailSender : IEmailSender
        {
            private readonly SendGridOptions _options;

            public SendGridEmailSender(IOptions<SendGridOptions> options)
            {
                _options = options.Value;
            }

            public async Task SendEmailAsync(string toEmail, string subject, string message)
            {
                var client = new SendGridClient(_options.ApiKey);
                var from = new EmailAddress(_options.SenderEmail, _options.SenderName);
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
