using System.Linq;
using DistribuidoraDoChines.Commons.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace DistribuidoraDoChines.Commons.Services
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            message.To.AddRange(emailMessage
                .ToAddresses
                .Select(x => new MailboxAddress(x.Name, x.Address)));

            message.From.Add(new MailboxAddress(_emailConfiguration.SmtpUsername));

            message.Subject = emailMessage.Subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            using var emailClient = new SmtpClient();

            emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort,
                _emailConfiguration.UseSsl);

            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

            emailClient.Send(message);

            emailClient.Disconnect(true);
        }
    }
}