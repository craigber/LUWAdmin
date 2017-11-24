using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class ChapterCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Chapter Name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "Secondary Name")]
        public string SubName { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(100)]
        [Display(Name = "Web Address")]
        public string Url { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Display(Name = "Location Name")]
        [MaxLength(100)]
        public string Venue { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100)]
        public string Street1 { get; set; }

        [MaxLength(100)]
        public string Street2 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(2)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        [Display(Name = "Week of Month")]
        public int? MeetingWeek { get; set; }

        [Display(Name = "Day of Week")]
        public int? MeetingDay { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        public string Notes { get; set; }

        public string Facebook { get; set; }
        public string Twitter { get; set; }

    }
}
