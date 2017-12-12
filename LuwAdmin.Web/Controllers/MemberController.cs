using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.DomainServices;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.MemberViewModels;
using LuwAdmin.Web.Models.SharedViewModels;
using LuwAdmin.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace LuwAdmin.Controllers
{
    [Authorize(Roles = "League")]

    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DomainService _domainService;
        //private readonly IEmailProcessingService _emailProcessor;

        public MemberController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager
            //IEmailProcessingService emailProcessor
            )
        {
            _context = context;
            _userManager = userManager;
            _domainService = new DomainService();
            //_emailProcessor = emailProcessor;
        }

        public async Task<string> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            return user.ContactName == "Legal Name" ? user.FirstName + user.LastName : user.Pseudonym;
        }

        public async Task<IActionResult> Index(string status = "Active")
        {
            ViewBag.User = await GetCurrentUser();

            var personTypes = await _context.PersonTypes.ToListAsync();
            var viewModel = new MemberIndexViewModel
            {
                Status = status,
                Members = new List<MemberIndexRow>()
            };

            IList<ApplicationUser> applicationUsers;
            var chapters = await _context.Chapters.ToListAsync();

            if (status == "All" || status == "Active")
            {
                applicationUsers = await _context.ApplicationUser
                    .Where(a  => a.WhenExpires.Date >= DateTime.Now.Date)
                    .Include(a => a.Chapters).ToListAsync();
            }
            else if (status == "Next30")
            {
                applicationUsers = await _context.ApplicationUser
                    .Where(a => a.WhenExpires.Date >= DateTime.Now.Date && a.WhenExpires.Date <= DateTime.Now.Date.AddDays(30))
                    .Include(a => a.Chapters).ToListAsync();
            }
            else if (status == "Last30")
            {
                applicationUsers = await _context.ApplicationUser
                    .Where(a => a.WhenJoined.Date >= DateTime.Now.Date.AddDays(-30))
                    .Include(a => a.Chapters).ToListAsync();
            }
            else if (status == "Left30")
                applicationUsers = await _context.ApplicationUser
                    .Where(a => a.WhenExpires.Date > DateTime.Now.Date.AddDays(-30)
                    && a.WhenExpires.Date < DateTime.Now.Date)
                    .Include(a => a.Chapters).ToListAsync();
            else
            {
                applicationUsers = await _context.ApplicationUser
                    .Where(a => a.Status == status)
                    .Include(a => a.Chapters).ToListAsync();
            }

            applicationUsers = applicationUsers.OrderBy(a => a.CommonName).ToList();

            foreach (var au in applicationUsers)
            {
                var personType = personTypes.FirstOrDefault(p => p.Id == au.PersonTypeId);

                viewModel.Members.Add(new MemberIndexRow
                {
                    Id = au.Id,
                    CommonName = au.CommonName,
                    FirstName = au.FirstName,
                    LastName = au.LastName,
                    Pseudonym = au.Pseudonym,
                    PersonType = personTypes.FirstOrDefault(p => p.Id == au.PersonTypeId),
                    Street1 = au.Street1,
                    Street2 = au.Street2,
                    City = au.City,
                    State = au.State,
                    ZipCode = au.ZipCode,
                    Status = au.Status,
                    Email = au.Email,
                    PersonTypeName = personType.Name,
                    WhenExpires = au.WhenExpires,
                    WhenJoined = au.WhenJoined,
                    IsExpiring = _domainService.Expiration.IsExpiring(personType, au.WhenExpires),
                    DaysToExpiration = _domainService.Expiration.DaysToExpiration(au.WhenExpires),
                    Chapters = FillChapters(au.Id, personType, chapters)
                });
            }

            var members = await _context.ApplicationUser.Where(a => a.WhenExpires.Date >= DateTime.Now.Date).ToListAsync();
            ViewBag.MemberCount = members.Count;
            ViewBag.Last30Days = members.Count(m => m.WhenJoined >= DateTime.Now.AddDays(-30));
            ViewBag.Next30Days = members.Count(m => m.WhenExpires >= DateTime.Now && m.WhenExpires <= DateTime.Now.AddDays(30) && m.Status == "Active");

            return View(viewModel);
        }
        
        private IList<MemberChapterIndexViewModel> FillChapters(string memberId, PersonType personType, List<Chapter> chapters)
        {
            var memberChapters = new List<MemberChapterIndexViewModel>();
            var mcs = _context.MemberChapters.Where(m => m.ApplicationUserId == memberId && m.WhenExpires >= DateTime.Now).ToList();
            foreach (var mc in mcs)
            {
                var chapter = chapters.FirstOrDefault(c => c.Id == mc.ChapterId);
                memberChapters.Add(
                    new MemberChapterIndexViewModel
                    {
                        ChapterId = chapter.Id,
                        Name = chapter.Name,
                        WhenExpires = mc.WhenExpires.Value,
                        IsPrimary = mc.IsPrimary,
                        IsExpiring = _domainService.Expiration.IsExpiring(personType, mc.WhenExpires.Value),
                        DaysToExpiration = _domainService.Expiration.DaysToExpiration(mc.WhenExpires.Value)
                    });
            }
            return memberChapters;
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var personType = await _context.PersonTypes.FindAsync(applicationUser.PersonTypeId);

            var memberChapters = await _context.MemberChapters
                .Include(mc => mc.Chapter)
                .Where(mc => mc.ApplicationUserId == applicationUser.Id && mc.WhenExpires >= DateTime.Now.Date.AddDays(-1 * personType.StopSendingRenewalDays))
                .ToListAsync();

            IList<MemberDetailsChapterViewModel> chapters = new List<MemberDetailsChapterViewModel>();
            foreach (var mc in memberChapters)
            {
                chapters.Add(new MemberDetailsChapterViewModel
                {
                    Id = mc.Id,
                    Name = mc.Chapter.Name,
                    WhenJoined = mc.WhenJoined,
                    WhenExpires = mc.WhenExpires.Value,
                    IsExpiring = _domainService.Expiration.IsExpiring(personType, mc.WhenExpires.Value),
                    DaysToExpiration = _domainService.Expiration.DaysToExpiration(mc.WhenExpires.Value),
                    IsPrimary = mc.IsPrimary
                });
            }

            var viewModel = new MemberDetailsViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Pseudonym = applicationUser.Pseudonym,
                ContactName = applicationUser.ContactName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                ZipCode = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires,
                IsExpiring = _domainService.Expiration.IsExpiring(personType, applicationUser.WhenExpires),
                DaysToExpiration = _domainService.Expiration.DaysToExpiration(applicationUser.WhenExpires),
                Notes = await _context.ApplicationUserNotes.Where(n => n.ApplicationUserId == applicationUser.Id).OrderByDescending(n => n.WhenAdded).ToListAsync(),
                Chapters = chapters,
                PersonType = personType,
                HasUpComingRenewals = chapters.Any(c => c.IsExpiring)
            };

            ViewBag.CommonName = applicationUser.CommonName;

            return View(viewModel);
        }

        public bool HasRenewal(ApplicationUser member, IList<MemberChapter> chapters, PersonType personType)
        {
            var hasRenewal = false;
            var renewalPeriodStarts = DateTime.Now.Date.AddDays(-1 * personType.StartSendingRenewalDays);
            var renewalPeriodEnds = DateTime.Now.Date.AddDays(personType.StopSendingRenewalDays);

            hasRenewal =
                (member.WhenExpires.Date >= renewalPeriodStarts && member.WhenExpires.Date <= renewalPeriodEnds)
                || chapters.Any(c => c.WhenExpires?.Date >= renewalPeriodStarts && c.WhenExpires?.Date <= renewalPeriodEnds);

            return hasRenewal;
        }

        // GET: Member/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.User = await GetCurrentUser();

            var personTypes = await _context.PersonTypes.OrderBy(p => p.Name).ToListAsync();
            
            var model = new MemberCreateViewModel
            {
                State = "UT",
                Status = "Active",
                IsSaveAndNew = false,
                WhenJoined = DateTime.Now,
                WhenExpires = DateTime.Now.AddYears(1),
                PersonTypeId = personTypes.FirstOrDefault(p => p.IsMemberDefault).Id
            };

            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = _context.Chapters.OrderBy(c => c.Name).ToList();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            model.Chapters = chapterSelect;

            var personTypesSelect = new List<SelectListItem>();
            foreach (var pt in personTypes)
            {
                personTypesSelect.Add(new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                });
            }
            model.PersonTypes = personTypesSelect;
            
            return View(model);
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City,Email,FirstName,LastName,State,Street1,Street2,Zip,Phone,Email,Note,WhenJoined,WhenExpires,Chapter,PersonTypeId,IsSaveAndNew,Pseudonym,ContactName")] MemberCreateViewModel viewModel)
        {
            ViewBag.User = await GetCurrentUser();
            
            if (_context.ApplicationUser.Any(a => a.Email == viewModel.Email))
            {
                ModelState.AddModelError("Email", "Email already in use");
            }

            var personTypes = await _context.PersonTypes.OrderBy(p => p.Name).ToListAsync();

            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    City = viewModel.City,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Pseudonym = viewModel.Pseudonym,
                    ContactName = viewModel.ContactName,
                    State = viewModel.State,
                    Street1 = viewModel.Street1,
                    ZipCode = viewModel.Zip,
                    PhoneNumber = viewModel.Phone,
                    WhenJoined = viewModel.WhenJoined,
                    WhenExpires = viewModel.WhenExpires,
                    Status = viewModel.Status,
                    UserName = viewModel.Email,
                    PersonTypeId = viewModel.PersonTypeId
                };

                var result = await _userManager.CreateAsync(applicationUser, "1We%1We%" + DateTime.UtcNow.ToString());
                
                if (!string.IsNullOrEmpty(viewModel.Note))
                {
                    var note = new ApplicationUserNote
                    {
                        ApplicationUserId = applicationUser.Id,
                        AddedBy = ViewBag.User, 
                        WhenAdded = DateTime.Now,
                        Note = viewModel.Note
                    };
                    _context.Add(note);
                }
                
                if (viewModel.Chapter != 0)
                {
                    _context.Add(new MemberChapter
                    {
                        ApplicationUserId = applicationUser.Id,
                        ChapterId = viewModel.Chapter,
                        WhenJoined = DateTime.Now
                    });
                }

                var personType = personTypes.FirstOrDefault(p => p.Id == viewModel.PersonTypeId);
                await _userManager.AddToRoleAsync(applicationUser, personType.Name);
              
                await _context.SaveChangesAsync();
                
                if (viewModel.IsSaveAndNew)
                {
                    return RedirectToAction("Create");
                }

                return RedirectToAction("Index");
            }
            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = _context.Chapters.OrderBy(c => c.Name).ToList();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            viewModel.Chapters = chapterSelect;
            viewModel.IsSaveAndNew = false;

            var personTypesSelect = new List<SelectListItem>();
            foreach (var pt in personTypes)
            {
                personTypesSelect.Add(new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                });
            }
            viewModel.PersonTypes = personTypesSelect;

            return View(viewModel);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.User = await GetCurrentUser();
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            
            var memberChapters = await _context.MemberChapters
                .Where(m => m.ApplicationUserId == id && m.WhenExpires >= DateTime.Now).ToListAsync();

            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = await _context.Chapters.OrderBy(c => c.Name).ToListAsync();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }

            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Member", Text = "Member" },
                new SelectListItem { Value = "Chapter", Text = "Chapter" },
                new SelectListItem { Value = "League", Text = "League" },
                new SelectListItem { Value = "Guest", Text = "Guest" }
            };

            var personTypes = await _context.PersonTypes.OrderBy(p => p.Name).ToListAsync();
            var personTypesSelect = new List<SelectListItem>();
            foreach (var pt in personTypes)
            {
                personTypesSelect.Add(new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                });
            }

            var viewModel = new MemberEditViewModel
            {

                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Pseudonym = applicationUser.Pseudonym,
                ContactName = applicationUser.ContactName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                Zip = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                Status = applicationUser.Status,
                WhenExpires = applicationUser.WhenExpires,
                PersonTypes = personTypesSelect,
            };

            ViewBag.CommonName = applicationUser.CommonName;

            return View(viewModel);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,City,Email,FirstName,LastName,State,Street1,Street2,Zip,Phone,Email,WhenJoined,WhenExpires,PersonTypeId,Pseudonym,ContactName")] MemberEditViewModel viewModel)
        {
            ViewBag.User = await GetCurrentUser();
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == viewModel.Id);
            if (viewModel.Id != applicationUser.Id)
            {
                return NotFound();
            }
            
            if (viewModel.WhenExpires < DateTime.Now && viewModel.Status == "Active")
            {
                ModelState.AddModelError("", "Status cannot be Active if Expiration Date is before today");
            }

            var currentUser = _context.ApplicationUser.FirstOrDefault(a => a.Email == viewModel.Email);
            if (currentUser != null && currentUser.Id != viewModel.Id)
            {
                ModelState.AddModelError("Email", "Email already in use");
            }

            var personTypes = await _context.PersonTypes.ToListAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.PersonTypeId != applicationUser.PersonTypeId)
                    {
                        var oldPersonType = personTypes.FirstOrDefault(p => p.Id == applicationUser.PersonTypeId);
                        var newPersonType = personTypes.FirstOrDefault(p => p.Id == viewModel.PersonTypeId);
                        if (oldPersonType != null)
                        {
                            await _userManager.RemoveFromRoleAsync(applicationUser, oldPersonType.SecurityGroup);
                        }
                        await _userManager.AddToRoleAsync(applicationUser, newPersonType.SecurityGroup);
                    }

                    applicationUser.City = viewModel.City;
                    applicationUser.Email = viewModel.Email;
                    applicationUser.FirstName = viewModel.FirstName;
                    applicationUser.LastName = viewModel.LastName;
                    applicationUser.Pseudonym = viewModel.Pseudonym;
                    applicationUser.ContactName = viewModel.ContactName;
                    applicationUser.Street1 = viewModel.Street1;
                    applicationUser.Street2 = viewModel.Street2;
                    applicationUser.City = viewModel.City;
                    applicationUser.State = viewModel.State;
                    applicationUser.ZipCode = viewModel.Zip;
                    applicationUser.Email = viewModel.Email;
                    applicationUser.PhoneNumber = viewModel.Phone;
                    applicationUser.WhenJoined = viewModel.WhenJoined;
                    applicationUser.WhenExpires = viewModel.WhenExpires;
                    applicationUser.UserName = viewModel.Email;
                    applicationUser.PersonTypeId = viewModel.PersonTypeId;
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                    
                    Response.Cookies.Append("FlashSuccess", "Member " + applicationUser.FirstName + " " + applicationUser.LastName + " was successfully saved");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            
                return RedirectToAction("Index");
            }

            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = await _context.Chapters.OrderBy(c => c.Name).ToListAsync();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            viewModel.Chapters = chapterSelect;

            var personTypesSelect = new List<SelectListItem>();
            foreach (var pt in personTypes)
            {
                personTypesSelect.Add(new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name
                });
            }

            ViewBag.CommonName = applicationUser.CommonName;
            return View(viewModel);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewBag.CommonName = applicationUser.CommonName;
            return View(applicationUser);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ViewBag.User = await GetCurrentUser();
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);

            var personType = await _context.PersonTypes.FindAsync(applicationUser.PersonTypeId);
            var identityResult = await _userManager.RemoveFromRoleAsync(applicationUser, personType.SecurityGroup);
            await _context.SaveChangesAsync();
            Response.Cookies.Append("FlashSuccess", "Member " + applicationUser.FirstName + " " + applicationUser.LastName + " was successfully deleted");
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Renew(string id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var viewModel = new MemberRenewViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Pseudonym = applicationUser.Pseudonym,
                ContactName = applicationUser.ContactName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                ZipCode = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires,
                NewWhenExpires = applicationUser.WhenExpires < DateTime.Now ? DateTime.Now.AddYears(1) : applicationUser.WhenExpires.AddYears(1),
            };
            ViewBag.CommonName = applicationUser.CommonName;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew([Bind("Id,NewWhenExpires")] MemberRenewViewModel viewModel)
        {

            ViewBag.User = await GetCurrentUser();
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == viewModel.Id);
            if (viewModel.Id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.WhenExpires = viewModel.NewWhenExpires;
                    if (applicationUser.WhenExpires < DateTime.Now)
                    {
                        await _userManager.RemoveFromRoleAsync(applicationUser, "League");
                        await _userManager.RemoveFromRoleAsync(applicationUser, "Chapter");
                        await _userManager.RemoveFromRoleAsync(applicationUser, "Member");
                        if (applicationUser.SecurityGroup != "Guest")
                        {
                            await _userManager.AddToRoleAsync(applicationUser, "Member");
                        }
                        applicationUser.SecurityGroup = "Member";
                    }

                    applicationUser.Status = "Active";
                    _context.Update(applicationUser);
                    Response.Cookies.Append("FlashSuccess", "Member " + applicationUser.FirstName + " " + applicationUser.LastName + " was successfully renewed");
                    await _context.SaveChangesAsync();

                    }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            viewModel = new MemberRenewViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Pseudonym = applicationUser.Pseudonym,
                ContactName = applicationUser.ContactName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                ZipCode = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires,
                NewWhenExpires = applicationUser.WhenExpires < DateTime.Now ? DateTime.Now.AddYears(1) : applicationUser.WhenExpires.AddYears(1)
            };
            ViewBag.CommonName = applicationUser.CommonName;
            return View(viewModel);
        }

        public async Task<IActionResult> SendEmailExpireNext30Days()
        {
            ViewBag.User = await GetCurrentUser();
            var now = DateTime.Now;
            var now30 = DateTime.Now.AddDays(30);
            var people = await _context.ApplicationUser.Where(a => a.WhenExpires >= now && a.WhenExpires <= now30 && a.Status == "Active").ToListAsync();
            var assignment = await _context.EmailAssignments.FirstOrDefaultAsync(e => e.Name == "Upcoming Expiration");
            var template = await _context.EmailTemplates.FirstOrDefaultAsync(t => t.EmailAssignmentId == assignment.Id);
            //_emailProcessor.Process(people, template, null);
            return View("EmailSent");
        }

        public async Task<IActionResult> AddNote(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var viewModel = new MemberNoteViewModel
            {
                ApplicationUserId = id,
                MemberName = applicationUser.ContactName == "Legal Name" ? applicationUser.FirstName + " " + applicationUser.LastName : applicationUser.Pseudonym
            };
            ViewBag.CommonName = applicationUser.CommonName;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote([Bind("ApplicationUserId,Text")] MemberNoteViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;

            var model = new ApplicationUserNote
            {
                ApplicationUserId = viewModel.ApplicationUserId,
                AddedBy = user?.FirstName + " " + user?.LastName,
                WhenAdded = DateTime.Now,
                Note = viewModel.Text
            };

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = viewModel.ApplicationUserId });            
        }

        public async Task<IActionResult> AddChapter(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var member = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);

            var allChapters = _context.Chapters.OrderBy(c => c.Name);
            var currentChapters = _context.MemberChapters.Where(c => c.ApplicationUserId == id && c.WhenExpires >= DateTime.Now);
            var chapters = _context.Chapters.Where(x => !currentChapters.Select(cc => cc.ChapterId).Contains(x.Id)).OrderBy(cc => cc.Name).ToList();
            var chapterSelect = new List<SelectListItem>();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }

            var viewModel = new MemberAddChapterViewModel
            {
                ApplicationUserId = id,
                WhenJoined = DateTime.Now,
                WhenExpires = DateTime.Now.AddYears(1),
                Chapters = chapterSelect,
                MemberName = member.ContactName == "Legal Name" ? member.FirstName + " " + member.LastName : member.Pseudonym
            };
            ViewBag.CommonName = member.CommonName;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChapter([Bind("ApplicationUserId,ChapterId,WhenJoined,WhenExpires")] MemberAddChapterViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == viewModel.ChapterId);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var model = new MemberChapter
            {
                ApplicationUserId = viewModel.ApplicationUserId,
                ChapterId = viewModel.ChapterId,
                WhenJoined = viewModel.WhenJoined,
                WhenExpires = viewModel.WhenExpires
            };

            _context.Add(model);

            var note = new ApplicationUserNote
            {
                ApplicationUserId = viewModel.ApplicationUserId,
                AddedBy = user?.FirstName + " " + user?.LastName,
                WhenAdded = DateTime.Now,
                Note = "Joined chapter " + chapter.Name
            };

            _context.Add(note);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = viewModel.ApplicationUserId });
        }

        public async Task<IActionResult> LeaveChapter(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;

            var memberChapter = await _context.MemberChapters.SingleOrDefaultAsync(m => m.Id == id);
            var member = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == memberChapter.ApplicationUserId);
            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == memberChapter.ChapterId);

            var viewModel = new MemberLeaveChapterViewModel
            {
                MemberChapterId = memberChapter.Id,
                MemberName = member.ContactName == "Legal Name" ? member.FirstName + " " + member.LastName : member.Pseudonym,
                ChapterName = chapter.Name
            };

            ViewBag.MemberName = member.FirstName + " " + member.LastName;
            ViewBag.CommonName = member.CommonName;
            return View(viewModel);
        }

        [HttpPost, ActionName("LeaveChapter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveChapterConfirmed([Bind("MemberChapterId")] int id)
        {
            var leavePrimaryText = "";
            var model = _context.MemberChapters.FirstOrDefault(m => m.Id == id);
            if (model.IsPrimary)
            {
                leavePrimaryText = "Removed Primary chapter.";
            }
            model.WhenExpires = DateTime.Now;
            model.IsPrimary = false;
            _context.Update(model);

            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == model.ChapterId);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
           
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == model.ApplicationUserId);

            var memberNote = new ApplicationUserNote
            {
                Note = "Removed from chapter " + chapter.Name + ". " + leavePrimaryText,
                ApplicationUserId = model.ApplicationUserId,
                AddedBy = user.FirstName + " " + user.LastName,
                WhenAdded = DateTime.Now
            };
            _context.ApplicationUserNotes.Add(memberNote);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = model.ApplicationUserId });
        }

        public async Task<IActionResult> ChangePrimaryChapter(string id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;

            var member = _context.ApplicationUser.FirstOrDefault(m => m.Id == id);
            var memberChapters = await _context.MemberChapters
                .Include(mc => mc.Chapter)
                .Where(mc => mc.ApplicationUserId == id && mc.WhenExpires >= DateTime.Now)
                .ToListAsync();

            var viewModel = new ChangePrimaryChapterViewModel
            {
                MemberId = id,
                MemberName = member.FirstName + " " + member.LastName,
                CurrentPrimary = "None",
                Chapters = new List<SelectListItem>()
            };
            
            foreach (var mc in memberChapters)
            {
                if (mc.IsPrimary)
                {
                    viewModel.MemberChapterId = mc.ChapterId;
                    viewModel.CurrentPrimary = mc.Chapter.Name;
                }
                else
                {
                    viewModel.Chapters.Add(new SelectListItem
                    {
                        Value = mc.Id.ToString(),
                        Text = mc.Chapter.Name
                    });
                }
            }
            ViewBag.CommonName = member.CommonName;
            return View(viewModel);
        }

        [HttpPost, ActionName("ChangePrimaryChapter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePrimaryChapter(
            [Bind("MemberId,MemberChapterId")] ChangePrimaryChapterViewModel viewModel)
        {
            var currentName = "None";
            var currentPrimary = await _context.MemberChapters.Include(mc => mc.Chapter).FirstOrDefaultAsync(mc => mc.ApplicationUserId == viewModel.MemberId && mc.IsPrimary);
            if (currentPrimary != null)
            {
                currentName = currentPrimary.Chapter.Name;
                currentPrimary.IsPrimary = false;
                _context.Update(currentPrimary);
            }

            var newPrimary = await _context.MemberChapters.Include(mc => mc.Chapter).FirstOrDefaultAsync(mc => mc.Id == viewModel.MemberChapterId);
            newPrimary.IsPrimary = true;
            _context.Update(newPrimary);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var memberNote = new ApplicationUserNote
            {
                Note = "Changed primary chapter from " + currentName 
                    + " to " + newPrimary.Chapter.Name,
                ApplicationUserId = viewModel.MemberId,
                AddedBy = user.FirstName + " " + user.LastName,
                WhenAdded = DateTime.Now
            };
            _context.ApplicationUserNotes.Add(memberNote);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = viewModel.MemberId });
        }
    }       
}


