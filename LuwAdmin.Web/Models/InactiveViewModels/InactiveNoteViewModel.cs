using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.InactiveViewModels
{
    public class InactiveNoteViewModel
    {
        public string ApplicationUserId { get; set; }

        [Display(Name = "Note")]
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }
        public string MemberName { get; set; }
    }
}
