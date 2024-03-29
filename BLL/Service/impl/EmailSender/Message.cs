using Microsoft.AspNetCore.Http;
using MimeKit;

namespace EmailSender;
public class Message
{
    public Message(string to, string subject, string content)
    {
        To = to;
        Subject = subject;
        Content = content;
    }

    public string To { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }
}