using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.ChapterViewModels
{
    public class ChapterDeleteMeetingViewModel
    {
        public int Id { get; set; }
        public string ChapterName { get; set; }
        public string MeetingType { get; set; }
        public string Description { get; set; }
        public string Venue { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string MeetingInfo { get; set; }
    }
}
