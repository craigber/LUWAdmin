using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class ChapterDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public IList<MeetingDetailsViewModel> Meetings { get; set; }
        public string Notes { get; set; }
        public IList<ChapterMemberViewModel> Members { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
    }
}
