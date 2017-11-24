using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.InactiveViewModels
{
    public class InactiveEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

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

        [DataType(DataType.Date)]
        [Display(Name = "First Joined")]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expired")]
        public DateTime WhenExpires { get; set; }
    }
}
