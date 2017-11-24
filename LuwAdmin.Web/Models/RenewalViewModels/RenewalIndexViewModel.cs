using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.RenewalViewModels
{
    public class RenewalIndexViewModel
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Pseudonym { get; set; }
        public string Email { get; set; }
        public string PersonTypeName { get; set; }
        public string LeagueRenewal { get; set; }
        public DateTime WhenExpires { get; set; }
        public DateTime? WhenLastRenewalSent { get; set; }
        public List<ChapterRenewals> Chapters { get; set; }
    }

    public class ChapterRenewals
    {
        public int MemberChapterId { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string WhenExpires { get; set; }
        public string WhenLastRenewalSent { get; set; }
    }
}
