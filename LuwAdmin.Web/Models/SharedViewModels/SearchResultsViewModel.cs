using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.SharedViewModels
{
    public class SearchResultsViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expires")]
        public DateTime WhenExpires { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
