using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace eIDEAS.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private string Host;
        private int Port;
        private bool EnableSSL;
        private string UserName;
        private string Password;
     

        public EmailSender(string host, int port, bool enableSSL, string username, string password)
        {
            this.Host = host;
            this.Port = port;
            this.EnableSSL = enableSSL;
            this.UserName = username;
            this.Password = password;
            
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
      
            };
            return client.SendMailAsync(
                new MailMessage(UserName, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}
