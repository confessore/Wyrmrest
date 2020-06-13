using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Wyrmrest.Web.Statics;

namespace Wyrmrest.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string recipient, string subject, string body)
        {
            return Execute(recipient, subject, body);
        }

        async Task Execute(string recipient, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                Host = EnvironmentVariables.SmtpHost,
                Port = Convert.ToInt32(EnvironmentVariables.SmtpPort),
                EnableSsl = true,
                Credentials = new NetworkCredential(EnvironmentVariables.SmtpUser, EnvironmentVariables.SmtpPass)
            };
            using var msg = new MailMessage(EnvironmentVariables.SmtpEmail, recipient)
            {
                From = new MailAddress(EnvironmentVariables.SmtpEmail, EnvironmentVariables.SmtpName),
                Subject = subject,
                Body = body
            };
            msg.IsBodyHtml = true;
            await smtpClient.SendMailAsync(msg);
        }
    }
}
