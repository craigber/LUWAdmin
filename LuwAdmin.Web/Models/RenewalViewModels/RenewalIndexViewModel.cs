using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.RenewalViewModels
{
    public class RenewalIndexViewModel
    {
        public string MemberId { get; set; }
        public string CommonName { get; set; }
        public string Name { get; set; }
        public string Pseudonym { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string PersonTypeName { get; set; }
        
        public IList<ChapterRenewal> Chapters { get; set; }

        public string Days { get; set; }

        public RenewalIndexViewModel()
        {
            Chapters = new List<ChapterRenewal>();
        }
    }

    public class ChapterRenewal
    {
        public int MemberChapterId { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenExpires { get; set; }

        public string Days { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WhenLastRenewalSent { get; set; }

        public List<string> IsRenewal { get; set; }
    }
}
