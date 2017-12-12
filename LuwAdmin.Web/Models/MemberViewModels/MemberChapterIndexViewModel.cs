using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberChapterIndexViewModel
    {
        public int ChapterId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenExpires { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsExpiring { get; set; }
        public int DaysToExpiration { get; set; }
    }
}
