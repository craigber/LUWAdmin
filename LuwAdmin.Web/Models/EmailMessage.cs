using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models
{
    public class EmailMessage
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string ReplyToEmail { get; set; }
        public string Body { get; set; }
    }
}
