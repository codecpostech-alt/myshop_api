using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailSettings = _config.GetSection("MailSettings");
            var from = mailSettings["From"];
            var host = mailSettings["Host"];
            var port = int.Parse(mailSettings["Port"]);
            var password = mailSettings["Password"];
            var enableSSL = bool.Parse(mailSettings["EnableSSL"]);

            using (var smtp = new SmtpClient(host, port))
            {
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.EnableSsl = enableSSL;

                var message = new MailMessage(from, toEmail, subject, body);
                message.IsBodyHtml = true;

                await smtp.SendMailAsync(message);
            }
        }
    }
}