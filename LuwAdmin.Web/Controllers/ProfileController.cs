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
using LuwAdmin.Web.Models.ProfileViewModels;
using LuwAdmin.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LuwAdmin.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailProcessingService _emailProcessor;

        public ProfileController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager 
            //IEmailProcessingService emailProcessor
            )
        {
            _context = context;
            _userManager = userManager;
            //_emailProcessor = emailProcessor;
        }

        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.FirstName + " " + user.LastName;

            var applicationUser = user;
            if (applicationUser == null)
            {
                return NotFound();
            }

            var chapters = await _context.MemberChapters
                .Include(mc => mc.Chapter)
                .Where(mc => mc.ApplicationUserId == applicationUser.Id && mc.WhenExpires >= DateTime.Now)
                .ToListAsync();

            var viewModel = new ProfileDetailsViewModel
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
                Notes = await _context.ApplicationUserNotes.Where(n => n.ApplicationUserId == applicationUser.Id).OrderByDescending(n => n.WhenAdded).ToListAsync(),
                Chapters = chapters,
                SecurityGroup = applicationUser.SecurityGroup
            };

            return View(viewModel);
        }
    }
}
