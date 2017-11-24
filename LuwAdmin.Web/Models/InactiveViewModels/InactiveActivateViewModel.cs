using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LuwAdmin.Web.Models.InactiveViewModels
{
    public class InactiveActivateViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenExpires { get; set; }
        
        [Display(Name ="New Expiration")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime NewWhenExpires { get; set; }

        [Display(Name = "Chapter")]
        public int ChapterId { get; set; }

        public List<SelectListItem> Chapters { get; set; }
    }
}
