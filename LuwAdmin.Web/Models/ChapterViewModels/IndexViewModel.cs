using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [Display(Name="Chapter Name")]
        public string Name { get; set; }

        [Display(Name="Meeting Info")]
        public string MeetingInfo { get; set; }

        public string City { get; set; }
    }
}
