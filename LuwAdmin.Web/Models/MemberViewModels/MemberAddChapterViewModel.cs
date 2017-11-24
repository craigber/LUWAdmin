using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;        
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberAddChapterViewModel
    {
        public string ApplicationUserId { get; set; }

        [Display(Name = "Chapter")]
        [Required]
        public int ChapterId { get; set; }

        public string MemberName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "When Joined")]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "When Expires")]
        public DateTime WhenExpires { get; set; }
        public List<SelectListItem> Chapters { get; set; }
    }
}
