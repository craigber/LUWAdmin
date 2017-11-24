using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Models;

namespace LuwAdmin.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
