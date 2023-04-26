using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Org.BouncyCastle.Crypto;
using System.Text;

namespace EmailSender;
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public void SendEmail(Message message)
    {
        var emailMassage = new MimeMessage();
        var from = _config["EmailConfiguration:From"];
        emailMassage.From.Add(new MailboxAddress(Encoding.UTF8, "Admin", from));
        emailMassage.To.Add(new MailboxAddress(message.To, message.To));
        emailMassage.Subject = message.Subject;
        string smtpServer = _config["EmailConfiguration:SmtpServer"];
        emailMassage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(message.Content)
        };
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(smtpServer, 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config["EmailConfiguration:From"], _config["EmailConfiguration:Password"]);
                client.Send(emailMassage);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}