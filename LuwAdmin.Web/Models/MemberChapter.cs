using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace LuwAdmin.Web.Models
{
    public class MemberChapter
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ChapterId { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WhenExpires { get; set; }

        public DateTime? WhenLastRenewalSent { get; set; }

        public bool IsPrimary { get; set; }
        public virtual Chapter Chapter { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
