using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Models;

namespace LuwAdmin.Web.Services
{
    public class EmailProcessingService : IEmailProcessingService
    {
        public void Process(IList<ApplicationUser> people, EmailTemplate template, IEmailSender emailSender)
        {
            foreach (var person in people)
            {
                var body = Merge(person, template);
                var email = new EmailMessage
                {
                    ToEmail = person.Email,
                    ToName = person.FirstName + " " + person.LastName,
                    FromEmail = template.SendFromEmailAddress,
                    FromName = template.SendFromName,
                    ReplyToEmail = template.ReplyToEmailAddress,
                    Subject = template.Subject,
                    Body = template.Body
                };
                var result = emailSender.SendEmailAsync(email);

            }
        }

        public string Merge(ApplicationUser person, EmailTemplate template)
        {
            var merged = template.Body;
            merged = merged.Replace("#FirstName#", person.FirstName);
            merged = merged.Replace("#LastName#", person.LastName);
            merged = merged.Replace("#ExpirationDate#", person.WhenExpires.ToString("MM/dd/yyyy"));
            return merged;
        }
    }
}
