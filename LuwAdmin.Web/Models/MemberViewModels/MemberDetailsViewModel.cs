﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.MemberViewModels
{
    public class MemberDetailsViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Pseudonym { get; set; }

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

        public bool IsExpiring { get; set; }

        public int DaysToExpiration { get; set; }
        public IList<ApplicationUserNote> Notes { get; set; }
        public IList<MemberDetailsChapterViewModel> Chapters { get; set; }
        public PersonType PersonType { get; set; }
        public bool HasUpComingRenewals { get; set; }

        public MemberDetailsViewModel()
        {
            Chapters = new List<MemberDetailsChapterViewModel>();
        }
    }

    public class MemberDetailsChapterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenJoined { get; set; }

        [DataType(DataType.Date)]
        public DateTime WhenExpires { get; set; }
        public bool IsExpiring { get; set; }
        public int DaysToExpiration { get; set; }
        public bool IsPrimary { get; set; }
    }
}
