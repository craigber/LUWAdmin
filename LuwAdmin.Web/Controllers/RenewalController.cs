using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.RenewalViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace LuwAdmin.Web.Controllers
{
    public class RenewalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailProcessingService _emailProcessor;

        public RenewalController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            //IEmailProcessingService emailProcessor
        )
        {
            _context = context;
            _userManager = userManager;
            //_emailProcessor = emailProcessor;
        }
        public async Task<IActionResult> Index()
        {
            var personTypes = await _context.PersonTypes.Where(p => p.StopSendingRenewalDays > 0 ||
                p.StartSendingRenewalDays > 0).ToListAsync();
            var members = new List<ApplicationUser>();
            var memberChapters = new List<MemberChapter>();

            foreach (var pt in personTypes)
            {
                members.AddRange(await _context.ApplicationUser.Where(a => a.PersonTypeId == pt.Id &&
                a.WhenExpires >= DateTime.Now.AddDays(-1 * pt.StartSendingRenewalDays) &&
                a.WhenExpires <= DateTime.Now.AddDays(pt.StopSendingRenewalDays)).ToListAsync());

                memberChapters.AddRange(await _context.MemberChapters.Where(m => m.WhenExpires >= DateTime.Now.AddDays(-1 * pt.StartSendingRenewalDays) &&
                m.WhenExpires <= DateTime.Now.AddDays(pt.StopSendingRenewalDays)).Include("Chapter")
                .Include("ApplicationUser").ToListAsync());
            }

            var viewModel = new List<RenewalIndexViewModel>();
            foreach (var member in members)
            {
                viewModel.Add(new RenewalIndexViewModel
                {
                    MemberId = member.Id,
                    Name = member.FirstName + " " + member.LastName,
                    Pseudonym = member.Pseudonym,
                    Email = member.Email,
                    PersonTypeName = personTypes.FirstOrDefault(p => p.Id == member.PersonTypeId).Name,
                    IsMembershipRenewal = true,
                    WhenExpires = member.WhenExpires,
                    WhenLastRenewalSent = member.WhenLastRenewalSent,
                    Days = DateTime.Now <= member.WhenExpires ? member.WhenExpires.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days" : DateTime.Now.Date.Subtract(member.WhenExpires.Date).Days.ToString() + " days ago"
                });
            }

            foreach (var chapter in memberChapters)
            {
                var member = viewModel.FirstOrDefault(m => m.MemberId == chapter.ApplicationUserId);

                var chapterRenew = new ChapterRenewal
                {
                    MemberChapterId = chapter.Id,
                    ChapterId = chapter.ChapterId,
                    Name = chapter.Chapter.Name,
                    IsPrimary = chapter.IsPrimary,
                    WhenExpires = chapter.WhenExpires.Value,
                    WhenLastRenewalSent = chapter.WhenLastRenewalSent,
                    Days = DateTime.Now <= chapter.WhenExpires ? chapter.WhenExpires.Value.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days" : DateTime.Now.Date.Subtract(chapter.WhenExpires.Value.Date).Days.ToString() + " days ago"
                };

                if (member == null)
                {
                    member = new RenewalIndexViewModel
                    {
                        MemberId = chapter.ApplicationUserId,
                        Name = chapter.ApplicationUser.FirstName + " " + chapter.ApplicationUser.LastName,
                        Pseudonym = chapter.ApplicationUser.Pseudonym,
                        Email = chapter.ApplicationUser.Email,
                        PersonTypeName = personTypes.FirstOrDefault(p => p.Id == chapter.ApplicationUser.PersonTypeId).Name,
                    };
                    member.Chapters.Add(chapterRenew);
                    viewModel.Add(member);
                }
                else
                {
                    member.Chapters.Add(chapterRenew);
                }

            }

            return View(viewModel);
        }
    }
}