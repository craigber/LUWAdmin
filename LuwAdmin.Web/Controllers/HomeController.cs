using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LuwAdmin.Web.Data;
using Microsoft.AspNetCore.Mvc;
using LuwAdmin.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LuwAdmin.Web.Controllers
{
    [Authorize(Roles = "League")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await GetCurrentUser();

            var chapters = await _context.Chapters.ToListAsync();
            ViewBag.ChapterCount = chapters.Count;

            var members = await _context.ApplicationUser.Where(a => a.Status == "Active").ToListAsync();
            ViewBag.MemberCount = members.Count;
            ViewBag.NewLast30Days = members.Count(m => m.WhenJoined >= DateTime.Now.AddDays(-30) && m.Status == "Active");
            ViewBag.Next30Days = members.Count(m => m.WhenExpires >= DateTime.Now && m.WhenExpires <= DateTime.Now.AddDays(30) && m.Status == "Active");
            ViewBag.LeftLast30Days =
                _context.ApplicationUser.Count(a => a.Status == "Inactive" &&
                                                    a.WhenExpires >= DateTime.Now.AddDays(-30) &&
                                                    a.WhenExpires <= DateTime.Now);
            return View();
        }

        public async Task<string> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;
            return user.ContactName == "Legal Name" ? user.FirstName + user.LastName : user.Pseudonym;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
