using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models
{
    public class PersonType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMember { get; set; }
        public bool IsMemberDefault { get; set; }
        public bool IsNonMemberDefault { get; set; }

        public string SecurityGroup { get; set; }
        public bool SendNewsletter { get; set; }
        public int NewsletterGraceDays { get; set; }
        public int StartSendingRenewalDays { get; set; }
        public int StopSendingRenewalDays { get; set; }
        public int MembershipFee { get; set; }
        public int ChapterSplit { get; set; }
        public int LeagueSplit { get; set; }
    }
}
