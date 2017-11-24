using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.MemberViewModels;
using LuwAdmin.Web.Models.SharedViewModels;
using LuwAdmin.Web.Services;
using LuwAdmin.Web.Models.InactiveViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace LuwAdmin.Controllers
{
    [Authorize(Roles = "League")]
    public class InactiveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailProcessingService _emailProcessor;

        public InactiveController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager) 
            //IEmailProcessingService emailProcessor)
        {
            _context = context;
            _userManager = userManager;
            //_emailProcessor = emailProcessor;
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
            
            var applicationUsers = await _context.ApplicationUser.Where(a => a.Status == "Inactive")
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync();

            var viewModel = new List<InactiveListViewModel>();
            foreach (var au in applicationUsers)
            {
                viewModel.Add(new InactiveListViewModel
                {
                    Id = au.Id,
                    FirstName = au.FirstName,
                    LastName = au.LastName,
                    City = au.City,
                    Email = au.Email,
                    WhenExpired = au.WhenExpires
                });
            }

            return View(viewModel);
        }
        
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
            
            var chapters = await _context.MemberChapters
                .Include(mc => mc.Chapter)
                .Where(mc => mc.ApplicationUserId == applicationUser.Id && mc.WhenExpires >= DateTime.Now)
                .ToListAsync();

            var viewModel = new InactiveDetailsViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
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
                Notes = await _context.ApplicationUserNotes.Where(n => n.ApplicationUserId == applicationUser.Id).OrderByDescending(n => n.WhenAdded).ToListAsync(),
            };

            return View(viewModel);
        }

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
            
            var viewModel = new InactiveEditViewModel
            {

                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                Zip = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires
            };

            return View(viewModel);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,City,Email,FirstName,LastName,State,Street1,Street2,Zip,Phone,Email,WhenJoined,WhenExpires")] InactiveEditViewModel viewModel)
        {
            ViewBag.User = await GetCurrentUser();
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == viewModel.Id);
            if (viewModel.Id != applicationUser.Id)
            {
                return NotFound();
            }

            var currentUser = _context.ApplicationUser.FirstOrDefault(a => a.Email == viewModel.Email);
            if (currentUser != null && currentUser.Id != viewModel.Id)
            {
                ModelState.AddModelError("Email", "Email already in use");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.Email = viewModel.Email;
                    applicationUser.FirstName = viewModel.FirstName;
                    applicationUser.LastName = viewModel.LastName;
                    applicationUser.Street1 = viewModel.Street1;
                    applicationUser.Street2 = viewModel.Street2;
                    applicationUser.City = viewModel.City;
                    applicationUser.State = viewModel.State;
                    applicationUser.ZipCode = viewModel.Zip;
                    applicationUser.Email = viewModel.Email;
                    applicationUser.PhoneNumber = viewModel.Phone;
                    applicationUser.UserName = viewModel.Email;
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
            return View(viewModel);
        }

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

            return View(applicationUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ViewBag.User = await GetCurrentUser();
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            var identityResult = await _userManager.RemoveFromRoleAsync(applicationUser, "League");
            await _context.SaveChangesAsync();
            Response.Cookies.Append("FlashSuccess", "Member " + applicationUser.FirstName + " " + applicationUser.LastName + " was successfully deleted");
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Activate(string id)
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

            var viewModel = new InactiveActivateViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
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
                Chapters = chapterSelect
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate([Bind("Id,NewWhenExpires,ChapterId")] InactiveActivateViewModel viewModel)
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
                    applicationUser.Status = "Active";
                    await _userManager.AddToRoleAsync(applicationUser, "Member");
                    _context.Update(applicationUser);

                    if (viewModel.ChapterId != 0)
                    {
                        _context.Add(new MemberChapter
                        {
                            ApplicationUserId = applicationUser.Id,
                            ChapterId = viewModel.ChapterId,
                            WhenJoined = DateTime.Now
                        });
                    }
                    await _context.SaveChangesAsync();
                    Response.Cookies.Append("FlashSuccess", "Member " + applicationUser.FirstName + " " + applicationUser.LastName + " was successfully renewed");
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
            var chapters = _context.Chapters.OrderBy(c => c.Name).ToList();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
  
            viewModel = new InactiveActivateViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                ZipCode = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires,
                NewWhenExpires = viewModel.WhenExpires < DateTime.Now ? DateTime.Now.AddYears(1) : applicationUser.WhenExpires.AddYears(1),
                Chapters = chapterSelect
            };

            return View(viewModel);
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

            var viewModel = new InactiveNoteViewModel
            {
                ApplicationUserId = id,
                MemberName = applicationUser.FirstName + " " + applicationUser.LastName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote([Bind("ApplicationUserId,Text")] InactiveNoteViewModel viewModel)
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

    }       
}


