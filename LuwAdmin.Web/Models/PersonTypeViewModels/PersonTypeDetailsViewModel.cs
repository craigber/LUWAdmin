using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.PersonTypeViewModels
{
    public class PersonTypeDetailsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Member")]
        public bool IsMember { get; set; }

        [Display(Name = "Member Default")]
        public bool IsMemberDefault { get; set; }

        [Display(Name = "Non-member Default")]
        public bool IsNonMemberDefault { get; set; }

        [Display(Name = "Security Group")]
        [Required]
        public string SecurityGroup { get; set; }

        [Display(Name = "Send Newsletter")]
        public bool SendNewsletter { get; set; }

        [Display(Name = "Newsletter Grace Period (days)")]
        public int NewsletterGraceDays { get; set; }

        [Display(Name = "Start Sending Renewal Emails (days)")]
        public int StartSendingRenewalDays { get; set; }

        [Display(Name = "Stop sending renewal emails (days)")]
        public int StopSendingRenewalDays { get; set; }

        [Display(Name = "Membership Dues")]
        public int MembershipFee { get; set; }

        [Display(Name = "Chapter Split")]
        public int ChapterSplit { get; set; }

        [Display(Name = "League Split")]
        public int LeagueSplit { get; set; }
    }
}
