using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name="Address")]
        [MaxLength(100)]
        public string Street1 { get; set; }
        [MaxLength(100)]
        public string Street2 { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(2)]
        public string State { get; set; }
        public string ZipCode { get; set; }

        [MaxLength(50)]
        [Display(Name="Outbound Email Server")]
        public string EmailServer { get; set; }

        [Display(Name = "Email Protocol")]
        public string EmailProtocol { get; set; }

        [Display(Name="Port")]
        [Range(0,65535)]
        public int EmailPort { get; set; }

        [Display(Name="Required Logon")]
        public bool DoesEmailRequireLogon { get; set; }

        [Display(Name="Email Logon")]
        [MaxLength(100)]
        public string EmailLogon { get; set; }

        [Display(Name="Email Password")]
        public string EmailPassword { get; set; }

        [Display(Name="Use SSL")]
        public bool EmailUseSsl { get; set; }
    }
}
