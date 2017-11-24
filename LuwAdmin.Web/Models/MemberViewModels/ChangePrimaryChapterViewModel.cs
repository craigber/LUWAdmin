using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class ChangePrimaryChapterViewModel
    {
        public string MemberId { get; set; }

        public string MemberName { get; set; }
        public int MemberChapterId { get; set; }

        [Display(Name = "Current Primary Chapter")]
        public string CurrentPrimary { get; set; }

        [Display(Name = "New Primary Chapter")]
        public List<SelectListItem> Chapters { get; set; }
    }
}
