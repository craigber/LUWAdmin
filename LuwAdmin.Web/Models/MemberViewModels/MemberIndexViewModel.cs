using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberIndexViewModel
    {
        public string Status { get; set; }
        public IList<MemberIndexRow> Members { get; set; }
    }

    public class MemberIndexRow
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Pseudonym { get; set; }

        public string CommonName { get; set; }
        public string PersonTypeName { get; set; }

        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Since")]
        public DateTime WhenJoined { get; set; }
        public IList<MemberChapterIndexViewModel> Chapters { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        [Display(Name = "Person Type")]
        public PersonType PersonType { get; set; }
        public bool IsExpiring { get; set; }
        public int DaysToExpiration { get; set; }

        public MemberIndexRow()
        {
            Chapters = new List<MemberChapterIndexViewModel>();
        }
    }
}
