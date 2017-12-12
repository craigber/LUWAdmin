using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Data.Migrations;
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

        public async Task<string> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            return user.ContactName == "Legal Name" ? user.FirstName + " " + user.LastName : user.Pseudonym;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await GetCurrentUser();

            var personTypes = await _context.PersonTypes.Where(p => p.StopSendingRenewalDays > 0 ||
                p.StartSendingRenewalDays > 0).ToListAsync();
            var members = new List<ApplicationUser>();
            var memberChapters = new List<MemberChapter>();

            foreach (var pt in personTypes)
            {
                members.AddRange(await _context.ApplicationUser.Where(a => a.PersonTypeId == pt.Id &&
                a.WhenExpires >= DateTime.Now.AddDays(-1 * pt.StartSendingRenewalDays) &&
                a.WhenExpires <= DateTime.Now.AddDays(pt.StopSendingRenewalDays)).ToListAsync());

                var renewingMemberChapters = await _context.MemberChapters
                    .Where(m => m.WhenExpires >= DateTime.Now.AddDays(-1 * pt.StartSendingRenewalDays)
                                && m.WhenExpires <= DateTime.Now.AddDays(pt.StopSendingRenewalDays))
                    .Include("Chapter").Include("ApplicationUser").ToListAsync();

                foreach (var mc in renewingMemberChapters)
                {
                    if (!memberChapters.Any(m => m.ApplicationUserId == mc.ApplicationUserId
                                                 && m.ChapterId == mc.ChapterId))
                    {
                        memberChapters.Add(mc);
                    }
                }
            }
            
            var viewModel = new List<RenewalIndexViewModel>();
            foreach (var member in members)
            {
                var renewMember = new RenewalIndexViewModel
                {
                    MemberId = member.Id,
                    Name = member.FirstName + " " + member.LastName,
                    Pseudonym = member.Pseudonym,
                    CommonName = member.CommonName,
                    Street1 = member.Street1,
                    Street2 = member.Street2,
                    City = member.City,
                    State = member.State,
                    ZipCode = member.ZipCode,
                    Email = member.Email,
                    PersonTypeName = personTypes.FirstOrDefault(p => p.Id == member.PersonTypeId).Name,
                    Days = DateTime.Now <= member.WhenExpires ? member.WhenExpires.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days" : DateTime.Now.Date.Subtract(member.WhenExpires.Date).Days.ToString() + " days ago"
                };

                var item = new ChapterRenewal
                {
                    Name = "League Membership",
                    WhenExpires = member.WhenExpires,
                    Days = DateTime.Now <= member.WhenExpires
                        ? member.WhenExpires.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days"
                        : DateTime.Now.Date.Subtract(member.WhenExpires.Date).Days.ToString() + " days ago",
                    WhenLastRenewalSent = member.WhenLastRenewalSent
                
                };
                renewMember.Chapters.Add(item);
                viewModel.Add(renewMember);
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

            var memberToRenewCount = 0;
            var memberExpiredCount = 0;
            var chapterToRenewCount = 0;
            var chapterExpiredCount = 0;
            var chapters = new List<string>();

            foreach (var member in viewModel)
            {
                foreach (var c in member.Chapters)
                {
                    if (c.Name == "League Membership" && c.WhenExpires.Date >= DateTime.Now.Date)
                    {
                        memberToRenewCount++;
                    }
                    else if (c.Name == "LeagueMembership")
                    {
                        memberExpiredCount++;
                    }
                    else if (c.WhenExpires.Date >= DateTime.Now.Date)
                    {
                        chapterToRenewCount++;
                    }
                    else
                    {
                        chapterExpiredCount++;
                    }

                    if (c.Name != "League Membership" && !chapters.Any(x => x == c.Name))
                    {
                        chapters.Add(c.Name);
                    }
                }
            }

            ViewBag.MemberToRenewCount = memberToRenewCount;
            ViewBag.MemberExpiredCount = memberExpiredCount;
            ViewBag.ChapterToRenewCount = chapterToRenewCount;
            ViewBag.ChapterExpiredCount = chapterExpiredCount;
            ViewBag.ChapterCount = chapters.Count;
            
            return View(viewModel.OrderBy(v => v.CommonName));
        }

        public async Task<IActionResult> Renew(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var member = await _context.ApplicationUser.Include("PersonType").FirstOrDefaultAsync(a => a.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            var personType = await _context.PersonTypes.FindAsync(member.PersonTypeId);

            var chapters = await _context.MemberChapters.Where(c => c.ApplicationUserId == id)
                .Include("Chapter").ToListAsync();

            var viewModel = new RenewViewModel
            {
                MemberId = member.Id,
                Name = member.FirstName + " " + member.LastName,
                Pseudonym = member.Pseudonym,
                CommonName = member.CommonName,
                Email = member.Email,
                PersonTypeName = member.PersonType.Name
            };

            var item = new RenewalItem
            {
                Name = "League Membership",
                WhenExpires = member.WhenExpires,
                IsRenewal =
                (member.WhenExpires >= DateTime.Now.AddDays(-1 * member.PersonType.StartSendingRenewalDays) &&
                 member.WhenExpires <= DateTime.Now.AddDays(member.PersonType.StopSendingRenewalDays)) ? 1 : 0,
                Days = DateTime.Now <= member.WhenExpires
                    ? member.WhenExpires.Date.Subtract(DateTime.Now.Date).Days
                    : DateTime.Now.Date.Subtract(member.WhenExpires.Date).Days,
                DaysText = DateTime.Now <= member.WhenExpires
                    ? member.WhenExpires.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days"
                    : DateTime.Now.Date.Subtract(member.WhenExpires.Date).Days.ToString() + " days ago",
                NewWhenExpires = member.WhenExpires.AddYears(1),
                ItemId = 0
            };
            viewModel.RenewalItems.Add(item);

            foreach (var chapter in chapters)
            {
                item = new RenewalItem
                {
                    Name = chapter.Chapter.Name,
                    IsPrimary = chapter.IsPrimary,
                    WhenExpires = chapter.WhenExpires.Value,
                    IsRenewal =  chapter.WhenExpires >=
                                 DateTime.Now.AddDays(-1 * member.PersonType.StartSendingRenewalDays) &&
                                 chapter.WhenExpires <= DateTime.Now.AddDays(member.PersonType.StopSendingRenewalDays) ? 1 : 0,
                    //IsRenewal = (chapter.WhenExpires >=
                    //             DateTime.Now.AddDays(-1 * member.PersonType.StartSendingRenewalDays) &&
                    //             chapter.WhenExpires <= DateTime.Now.AddDays(member.PersonType.StopSendingRenewalDays)),
                    Days = DateTime.Now <= chapter.WhenExpires
                        ? chapter.WhenExpires.Value.Date.Subtract(DateTime.Now.Date).Days
                        : DateTime.Now.Date.Subtract(chapter.WhenExpires.Value.Date).Days * -1,
                    DaysText = DateTime.Now <= chapter.WhenExpires
                        ? chapter.WhenExpires.Value.Date.Subtract(DateTime.Now.Date).Days.ToString() + " days"
                        : DateTime.Now.Date.Subtract(chapter.WhenExpires.Value.Date).Days.ToString() + " days ago",
                    NewWhenExpires = chapter.WhenExpires.Value.AddYears(1),
                    ItemId = chapter.Id
                };
                viewModel.RenewalItems.Add(item);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(string MemberId, List<int> item_IsRenewal, List<int> item_ItemId, List<DateTime> item_NewWhenExpires)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var member = await _context.ApplicationUser.FindAsync(MemberId);
            var chapters = await _context.Chapters.ToListAsync();

            for (var i = 0; i < item_IsRenewal.Count; i++)
            {
                if (item_IsRenewal[i] == 1)
                {
                    if (item_ItemId[i] == 0)
                    {
                        member.WhenExpires = item_NewWhenExpires[i];
                        // TODO Change WhenLastRenewalSent to nullable
                        // member.WhenLastRenewalSent = null;
                        _context.Update(member);

                        var note = new ApplicationUserNote
                        {
                            ApplicationUserId = MemberId,
                            AddedBy = user?.FirstName + " " + user?.LastName,
                            WhenAdded = DateTime.Now,
                            Note = "Renewed league membership."
                        };
                        _context.Add(note);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var memberChapter = await _context.MemberChapters.FindAsync(item_ItemId[i]);
                        memberChapter.WhenExpires = item_NewWhenExpires[i];
                        memberChapter.WhenLastRenewalSent = null;
                        _context.Update(memberChapter);

                        var chapter = chapters.FirstOrDefault(c => c.Id == memberChapter.ChapterId);

                        var note = new ApplicationUserNote
                        {
                            ApplicationUserId = MemberId,
                            AddedBy = user?.FirstName + " " + user?.LastName,
                            WhenAdded = DateTime.Now,
                            Note = "Renewed chapter " + chapter.Name + "membership."
                        };
                        _context.Add(note);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}