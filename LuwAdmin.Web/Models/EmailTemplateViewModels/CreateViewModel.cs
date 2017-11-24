using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LuwAdmin.Web.Models.EmailTemplateViewModels
{
    public class CreateViewModel
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name="Subject")]
        [Required]
        public string Subject { get; set; }

        [Display(Name="Body")]
        [Required]
        public string Body { get; set; }

        [Display(Name="Use For")]
        public int EmailAssignmentId { get; set; }
        public List<SelectListItem> EmailAssignments { get; set; }

        [Display(Name = "Send From Name")]
        [Required]
        public string SendFromName { get; set; }

        [Display(Name = "Send From Email")]
        [Required]
        [EmailAddress]
        public string SendFromEmailAddress { get; set; }

        [Display(Name = "Reply To Email")]
        [EmailAddress]
        public string ReplyToEmailAddress { get; set; }
    }
}
