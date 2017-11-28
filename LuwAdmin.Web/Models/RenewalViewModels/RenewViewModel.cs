using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.RenewalViewModels
{
    public class RenewViewModel
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Pseudonym { get; set; }
        public string CommonName { get; set; }
        public string Email { get; set; }
        public string PersonTypeName { get; set; }
        public IList<RenewalItem> RenewalItems { get; set; }

        public RenewViewModel()
        {
            RenewalItems = new List<RenewalItem>();
        }
    }

    public class RenewalItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenExpires { get; set; }

        public bool IsRenewal { get; set; }

        public int Days { get; set; }

        public string DaysText { get; set; }

        [DataType(DataType.Date)]
        public DateTime NewWhenExpires { get; set; }
    }
}
