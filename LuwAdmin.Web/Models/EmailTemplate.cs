using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models
{
    public class EmailTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int EmailAssignmentId { get; set; }

        public string SendFromName { get; set; }
        public string SendFromEmailAddress { get; set; }
        public string ReplyToEmailAddress { get; set; }
    }
}
