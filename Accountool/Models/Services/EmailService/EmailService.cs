using Accountool.Models.Services.EmailService.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Accountool.Models.Services.EmailService
{
    public interface IEmailService
    {
        Task<MimeMessage> PrepareSupportEmailMessage(IEnumerable<string> to, string subject, FeedbackModel feedbackModel);
        Task SendEmailAsync(MimeMessage emailMessage);
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

        public async Task<MimeMessage> PrepareSupportEmailMessage(IEnumerable<string> to, string subject, FeedbackModel feedbackModel)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_fromAddressTitle, _fromAddress));

            foreach (var recipient in to)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = $"<h2>Message: {feedbackModel.Message}</h2><br><h3>Name: {feedbackModel.Name}</h3><br><h4>Email: {feedbackModel.Email}</h4>" };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        public async Task SendEmailAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_username, _password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
