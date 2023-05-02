using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender;

public interface IEmailService
{
    Task<bool> SendEmail(Message message);
}