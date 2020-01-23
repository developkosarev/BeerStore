using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace BeerStore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string Email = _configuration["MailSender:Email"];
            string UserName = _configuration["MailSender:UserName"];
            string Password = _configuration["MailSender:Password"];
            string Host = _configuration["MailSender:Host"];
            int Port = Int32.Parse(_configuration["MailSender:Port"]);
            string TestSecret = _configuration["MailSender:TestSecret"];

            string newMessage = message + " => " + TestSecret;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта BeerStore", Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.To.Add(new MailboxAddress("", "it@chance-ltd.ru"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = newMessage
            };

            using (var client = new SmtpClient())
            {                
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(Host, Port, true);

                //client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(UserName, Password);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }       
    }
}
