using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CRM.API.Interface
{
    public interface IApplicatiomEmailSender
    {
        Task SendEmailAsync(MailMessage message);
    }
}