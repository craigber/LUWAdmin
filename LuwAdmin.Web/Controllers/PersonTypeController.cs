using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.PersonTypeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuwAdmin.Web.Controllers
{
    [Authorize(Roles = "League")]
    public class PersonTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonTypeController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            return user.ContactName == "Legal Name" ? user.FirstName + user.LastName : user.Pseudonym;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await GetCurrentUser();

            var personTypes = await _context.PersonTypes.OrderBy(p => p.Name).ToListAsync();
            var viewModel = new PersonTypeIndexViewModel();
            if (personTypes == null)
            {
                viewModel.PersonTypes = new List<PersonType>();
            }
            else
            {
                viewModel.PersonTypes = personTypes;
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.User = await GetCurrentUser();
            var viewModel = new PersonTypeCreateViewModel();

            return View(viewModel);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsMember,IsMemberDefault,IsNonMemberDefault,SecurityGroup,SendNewsletter,NewsletterGraceDays,StartSendingRenewalDays,StopSendingRenewalDays,MembershipFee,LeagueSplit,ChapterSplit")] PersonTypeCreateViewModel viewModel)
        {
            var isDuplicate = await _context.PersonTypes.AnyAsync(p => p.Name == viewModel.Name);
            if (isDuplicate)
            {
                ModelState.AddModelError("", "Person Type " + viewModel.Name + " already exists. It must be unique.");
            }

            if (ModelState.IsValid)
            {
                var personType = new PersonType
                {
                    Name = viewModel.Name,
                    IsMember = viewModel.IsMember,
                    IsMemberDefault = viewModel.IsMemberDefault,
                    IsNonMemberDefault = viewModel.IsNonMemberDefault,
                    SecurityGroup = viewModel.SecurityGroup,
                    SendNewsletter = viewModel.SendNewsletter,
                    NewsletterGraceDays = viewModel.NewsletterGraceDays,
                    StartSendingRenewalDays = viewModel.StartSendingRenewalDays,
                    StopSendingRenewalDays = viewModel.StopSendingRenewalDays,
                    MembershipFee = viewModel.MembershipFee,
                    LeagueSplit = viewModel.LeagueSplit,
                    ChapterSplit = viewModel.ChapterSplit
                };

                _context.Add(personType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var personType = await _context.PersonTypes.FindAsync(id);
            if (personType == null)
            {
                return NotFound();
            }

            ViewBag.User = await GetCurrentUser();
            var viewModel = new PersonTypeDetailsViewModel
            {
                Id = personType.Id,
                Name = personType.Name,
                IsMember = personType.IsMember,
                IsMemberDefault = personType.IsMemberDefault,
                IsNonMemberDefault = personType.IsNonMemberDefault,
                SecurityGroup = personType.SecurityGroup,
                SendNewsletter = personType.SendNewsletter,
                NewsletterGraceDays = personType.NewsletterGraceDays,
                StartSendingRenewalDays = personType.StartSendingRenewalDays,
                StopSendingRenewalDays = personType.StopSendingRenewalDays,
                MembershipFee = personType.MembershipFee,
                LeagueSplit = personType.LeagueSplit,
                ChapterSplit = personType.ChapterSplit
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var personType = await _context.PersonTypes.FirstOrDefaultAsync(p => p.Id == id);

            if (personType == null)
            {
                return NotFound();
            }

            ViewBag.User = await GetCurrentUser();
            var viewModel = new PersonTypeEditViewModel
            {
                Id = personType.Id,
                Name = personType.Name,
                IsMember = personType.IsMember,
                IsMemberDefault = personType.IsMemberDefault,
                IsNonMemberDefault = personType.IsNonMemberDefault,
                SecurityGroup = personType.SecurityGroup,
                SendNewsletter = personType.SendNewsletter,
                NewsletterGraceDays = personType.NewsletterGraceDays,
                StartSendingRenewalDays = personType.StartSendingRenewalDays,
                StopSendingRenewalDays = personType.StopSendingRenewalDays,
                MembershipFee = personType.MembershipFee,
                LeagueSplit = personType.LeagueSplit,
                ChapterSplit = personType.ChapterSplit
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,IsMember,IsMemberDefault,IsNonMemberDefault,SecurityGroup,SendNewsletter,NewsletterGraceDays,StartSendingRenewalDays,StopSendingRenewalDays,MembershipFee,LeagueSplit,ChapterSplit")] PersonTypeEditViewModel viewModel)
        {
            var isDuplicate = await _context.PersonTypes.AnyAsync(p => p.Name == viewModel.Name && p.Id != viewModel.Id);
            if (isDuplicate)
            {
                ModelState.AddModelError("", "Person Type " + viewModel.Name + " already exists. It must be unique.");
            }

            if (ModelState.IsValid)
            {
                var personType = await _context.PersonTypes.FindAsync(viewModel.Id);

                personType.Name = viewModel.Name;
                personType.IsMember = viewModel.IsMember;
                personType.IsMemberDefault = viewModel.IsMemberDefault;
                personType.IsNonMemberDefault = viewModel.IsNonMemberDefault;
                personType.SecurityGroup = viewModel.SecurityGroup;
                personType.SendNewsletter = viewModel.SendNewsletter;
                personType.NewsletterGraceDays = viewModel.NewsletterGraceDays;
                personType.StartSendingRenewalDays = viewModel.StartSendingRenewalDays;
                personType.StopSendingRenewalDays = viewModel.StopSendingRenewalDays;
                personType.MembershipFee = viewModel.MembershipFee;
                personType.LeagueSplit = viewModel.LeagueSplit;
                personType.ChapterSplit = viewModel.ChapterSplit;

                _context.Update(personType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var personType = await _context.PersonTypes.FindAsync(id);

            if (personType == null)
            {
                return NotFound();
            }

            ViewBag.User = await GetCurrentUser();
            ViewBag.Name = personType.Name;
            return View(personType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.User = await GetCurrentUser();
            var personType = await _context.PersonTypes.FindAsync(id);
            _context.PersonTypes.Remove(personType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}