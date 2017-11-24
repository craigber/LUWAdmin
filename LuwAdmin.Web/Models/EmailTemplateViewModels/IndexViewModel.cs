using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LuwAdmin.Web.Models.EmailTemplateViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name="Subject")]
        public string Subject { get; set; }

        [Display(Name="Use For")]
        public string Assignment { get; set; }
    }
}
