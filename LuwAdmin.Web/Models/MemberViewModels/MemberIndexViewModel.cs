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
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }
        public IList<MemberChapterIndexViewModel> Chapters { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        [Display(Name = "Person Type")]
        public PersonType PersonType { get; set; }

        public MemberIndexRow()
        {
            Chapters = new List<MemberChapterIndexViewModel>();
        }
    }
}
