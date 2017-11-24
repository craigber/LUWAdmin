using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.InactiveViewModels
{
    public class InactiveDetailsViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string ContactName { get; set; }
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
        public IList<ApplicationUserNote> Notes { get; set; }
        
    }
}
