using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.EmailTemplateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LuwAdmin.Web.Controllers
{
    [Authorize(Roles = "League")]
    public class EmailTemplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailTemplateController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EmailTemplates
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var models = await _context.EmailTemplates.ToListAsync();
            var assignments = await _context.EmailAssignments.ToListAsync();
            var viewModel = new List<IndexViewModel>();
            foreach (var model in models)
            {
                viewModel.Add(new IndexViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Subject = model.Subject,
                    Assignment = assignments.FirstOrDefault(a => a.Id == model.EmailAssignmentId).Name
                });
            }
            return View(viewModel);
        }


        // GET: EmailTemplates/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var viewModel = new CreateViewModel();
            var assignmentSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var assignments = _context.EmailAssignments.OrderBy(c => c.Name).ToList();
            foreach (var assignment in assignments)
            {
                assignmentSelect.Add(new SelectListItem
                {
                    Value = assignment.Id.ToString(),
                    Text = assignment.Name
                });
            }
            viewModel.EmailAssignments = assignmentSelect;
            return View(viewModel);
        }

        // POST: EmailTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Body,Name,Subject,EmailTemplateId,SendFromName,SendFromEmailAddress,ReplyToEmailAddress")] CreateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            if (ModelState.IsValid)
            {
                var emailTemplate = new EmailTemplate
                {
                    Name = viewModel.Name,
                    Subject = viewModel.Subject,
                    Body = viewModel.Body,
                    EmailAssignmentId = viewModel.EmailAssignmentId,
                    SendFromEmailAddress = viewModel.SendFromEmailAddress,
                    SendFromName = viewModel.SendFromName,
                    ReplyToEmailAddress = viewModel.ReplyToEmailAddress
                };
                _context.Add(emailTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var assignmentSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var assignments = _context.EmailAssignments.OrderBy(c => c.Name).ToList();
            foreach (var assignment in assignments)
            {
                assignmentSelect.Add(new SelectListItem
                {
                    Value = assignment.Id.ToString(),
                    Text = assignment.Name
                });
            }
            viewModel.EmailAssignments = assignmentSelect;
            return View(viewModel);
        }

        // GET: EmailTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            if (id == null)
            {
                return NotFound();
            }

            var emailTemplate = await _context.EmailTemplates.SingleOrDefaultAsync(m => m.Id == id);
            if (emailTemplate == null)
            {
                return NotFound();
            }
            var viewModel = new EditViewModel
            {
                Id = emailTemplate.Id,
                Name = emailTemplate.Name,
                Subject = emailTemplate.Subject,
                Body = emailTemplate.Body,
                EmailAssignmentId = emailTemplate.EmailAssignmentId,
                SendFromEmailAddress = emailTemplate.SendFromEmailAddress,
                SendFromName = emailTemplate.SendFromName,
                ReplyToEmailAddress = emailTemplate.ReplyToEmailAddress
            };

            var assignmentSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var assignments = _context.EmailAssignments.OrderBy(c => c.Name).ToList();
            foreach (var assignment in assignments)
            {
                assignmentSelect.Add(new SelectListItem
                {
                    Value = assignment.Id.ToString(),
                    Text = assignment.Name
                });
            }
            viewModel.EmailAssignments = assignmentSelect;
            return View(viewModel);
        }

        // POST: EmailTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body,Name,Subject,EmailAssignmentId,SendFromName,SendFromEmailAddress,ReplyToEmailAddress")] EditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var emailTemplate = await _context.EmailTemplates.SingleOrDefaultAsync(e => e.Id == viewModel.Id);
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    emailTemplate.Name = viewModel.Name;
                    emailTemplate.Subject = viewModel.Subject;
                    emailTemplate.Body = viewModel.Body;
                    emailTemplate.EmailAssignmentId = viewModel.EmailAssignmentId;
                    emailTemplate.SendFromEmailAddress = viewModel.SendFromEmailAddress;
                    emailTemplate.SendFromName = viewModel.SendFromName;
                    emailTemplate.ReplyToEmailAddress = viewModel.ReplyToEmailAddress;
                    _context.Update(emailTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailTemplateExists(emailTemplate.Id))
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

            var assignmentSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var assignments = _context.EmailAssignments.OrderBy(c => c.Name).ToList();
            foreach (var assignment in assignments)
            {
                assignmentSelect.Add(new SelectListItem
                {
                    Value = assignment.Id.ToString(),
                    Text = assignment.Name
                });
            }
            viewModel.EmailAssignments = assignmentSelect;
            return View(viewModel);
        }

        // GET: EmailTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            if (id == null)
            {
                return NotFound();
            }

            var emailTemplate = await _context.EmailTemplates.SingleOrDefaultAsync(m => m.Id == id);
            if (emailTemplate == null)
            {
                return NotFound();
            }

            return View(emailTemplate);
        }

        // POST: EmailTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            var emailTemplate = await _context.EmailTemplates.SingleOrDefaultAsync(m => m.Id == id);
            _context.EmailTemplates.Remove(emailTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EmailTemplateExists(int id)
        {
            return _context.EmailTemplates.Any(e => e.Id == id);
        }
    }
}
