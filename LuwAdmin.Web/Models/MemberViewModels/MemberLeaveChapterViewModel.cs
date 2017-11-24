using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberLeaveChapterViewModel
    {
        public int MemberChapterId { get; set; }
        public string MemberName { get; set; }
        public string ChapterName { get; set; }
    }
}
