using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Accountool.Models.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(IEnumerable<string> to, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromAddress;
        private readonly string _fromAddressTitle;
        private readonly string _username;
        private readonly string _password;

        public EmailService(string smtpServer, int smtpPort, string fromAddress, string fromAddressTitle, string username, string password)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromAddress = fromAddress;
            _fromAddressTitle = fromAddressTitle;
            _username = username;
            _password = password;
        }

        public async Task SendEmailAsync(IEnumerable<string> to, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_fromAddressTitle, _fromAddress));

            foreach (var recipient in to)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = subject;


            var bodyBuilder = new BodyBuilder { HtmlBody = $"<h2>{message}</h2>" };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, true);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
