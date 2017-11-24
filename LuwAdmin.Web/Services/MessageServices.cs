using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;

//namespace LuwAdmin.Web.Services
//{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    //public class AuthMessageSender : IEmailSender, ISmsSender
    //{
    //    private readonly ApplicationDbContext _context;

    //    public AuthMessageSender(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

        //public Task SendEmailAsync(string toEmail, string subject, string body)
        //{
        //    var emailMessage = new EmailMessage
        //    {
        //        ToEmail = toEmail,
        //        Subject = subject,
        //        Body = body
        //    };
        //    return SendEmailAsync(emailMessage);
        //}

        //public Task SendEmailAsync(EmailMessage message)
        //{
        //    //var tenant = _context.Tenants.FirstOrDefault();
            
            //// Plug in your email service here to send an email.
            //var emailMessage = new MimeKit.MimeMessage();
            //emailMessage.From.Add(new MailboxAddress(message.FromName, message.FromEmail));
            //emailMessage.To.Add(new MailboxAddress(message.ToName, message.ToEmail));
            //emailMessage.ReplyTo.Add(new MailboxAddress(message.ReplyToEmail, message.ReplyToEmail));
            //emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart("plain")
            //{
            //    Text = message.Body
            //};

            //using (var smtpClient = new SmtpClient())
            //{
            //    smtpClient.Connect(tenant.EmailServer, tenant.EmailPort, tenant.EmailUseSsl);
            //    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //    smtpClient.Authenticate(tenant.EmailLogon, tenant.EmailPassword);
            //    smtpClient.Send(emailMessage);
            //    smtpClient.Disconnect(true);
            //}

        //    return Task.FromResult(0);
        //}

//        public Task SendSmsAsync(string number, string message)
//        {
//            // Plug in your SMS service here to send a text message.
//            return Task.FromResult(0);
//        }
//    }
//}
