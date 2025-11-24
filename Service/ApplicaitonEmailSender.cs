using System.Net.Mail;
using CRM.API.Interface;
using CRM.API.Model;
using Microsoft.Extensions.Options;
namespace CRM.API.Service
{
    public class ApplicaitonEmailSender(IOptions<Emailconfiguration> emailConfiguration) : IApplicatiomEmailSender
    {
       private readonly Emailconfiguration _emailConfiguration = emailConfiguration.Value;

        public Task SendEmailAsync(MailMessage message)
        {
            message.From = new MailAddress(_emailConfiguration.FromEmail, _emailConfiguration.FromName);
            SmtpClient smtpClient = new(_emailConfiguration.SmtpServer, _emailConfiguration.Port)
            {
                Credentials = new System.Net.NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password),
                EnableSsl = true
            };
            return smtpClient.SendMailAsync(message);
        }
    }
}