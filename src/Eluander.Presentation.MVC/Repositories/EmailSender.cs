using Eluander.Presentation.MVC.Models;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Eluander.Presentation.MVC.Repositories
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message)
        {
            string toEmail = string.IsNullOrEmpty(email)
                ? _emailSettings.ToEmail
                : email;

            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.From, _emailSettings.FromUserName)
            };

            mail.To.Add(new MailAddress(toEmail));
            mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

            mail.Subject = "Eluander.com.br - " + subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            //outras opções
            //mail.Attachments.Add(new Attachment(arquivo));

            using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
            {
                smtp.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.PasswordUsername);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
