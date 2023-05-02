using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailSender;
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _config = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmail(Message message)
    {
        var emailMassage = new MimeMessage();
        emailMassage.From.Add(new MailboxAddress(Encoding.UTF8, "Admin", _config["EmailConfiguration:From"]));
        emailMassage.To.Add(new MailboxAddress(message.To, message.To));
        emailMassage.Subject = message.Subject;
        emailMassage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(message.Content),
        };
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_config["EmailConfiguration:SmtpServer"], 587, SecureSocketOptions.StartTls);
                client.Authenticate(_config["EmailConfiguration:From"], _config["EmailConfiguration:Password"]);

                await client.SendAsync(emailMassage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error in sending email for user {message.To}");
                return false;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}