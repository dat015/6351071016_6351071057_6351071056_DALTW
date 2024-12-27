using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TechShop.Services.Mail
{
    public static class SendMail
    {
        private static IConfiguration Configuration { get; set; }

        static SendMail()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public static bool SendEmail(string to, string subject, string body, string attachFile)
        {
            try
            {
                var emailSender = Configuration["EmailSettings:EmailSender"];
                var host = Configuration["EmailSettings:Host"];
                var port = int.Parse(Configuration["EmailSettings:Port"]);
                var passwordSender = Configuration["EmailSettings:PasswordSender"];

                MailMessage message = new MailMessage(emailSender, to, subject, body);
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(host, port))
                {
                    smtpClient.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        message.Attachments.Add(attachment);
                    }

                    NetworkCredential credentials = new NetworkCredential(emailSender, passwordSender);
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
                return false;
            }
            return true;
        }
    }
}