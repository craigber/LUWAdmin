using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Models;

namespace LuwAdmin.Web.Services
{
    public interface IEmailProcessingService
    {
        void Process(IList<ApplicationUser> people, EmailTemplate template, IEmailSender emailSender);
    }
}
