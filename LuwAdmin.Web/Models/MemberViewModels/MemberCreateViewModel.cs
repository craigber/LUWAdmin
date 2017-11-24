using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberCreateViewModel
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Pen Name")]
        public string Pseudonym { get; set; }

        [Display(Name = "Use This Name")]
        public string ContactName { get; set; }

        [Display(Name = "Address")]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set;}

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Member Since")]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }

        [Display(Name = "Chapter")]
        public int Chapter { get; set; }

        public List<SelectListItem> Chapters { get; set; }

        [Display(Name = "Person Type")]
        [Required]
        public int PersonTypeId { get; set; }

        public string Note { get; set; }

        public bool IsSaveAndNew { get; set; }

        [EmailAddress]
        public string PayPalEmail { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "When Paid")]
        public DateTime? WhenPaid { get; set; }

        public List<SelectListItem> PersonTypes { get; set; }
    }
}
