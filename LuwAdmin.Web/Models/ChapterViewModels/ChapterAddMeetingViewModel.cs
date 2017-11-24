using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class ChapterAddMeetingViewModel
    {
        public int ChapterId { get; set; }

        [Display(Name = "Location Name")]
        [MaxLength(100)]
        [Required]
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

        [Required]
        [Display(Name = "Meeting Type")]
        public string MeetingType { get; set; }

        public string Description { get; set; }
    }
}