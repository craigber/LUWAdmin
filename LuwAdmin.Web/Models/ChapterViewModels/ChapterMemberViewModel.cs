using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class ChapterMemberViewModel
    {
        [Display(Name ="Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Member Since")]
        public DateTime WhenJoined { get; set; }
        public string Email { get; set; }
    }
}
