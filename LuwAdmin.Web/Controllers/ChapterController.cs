using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuwAdmin.Web.Data;
using LuwAdmin.Web.Models;
using LuwAdmin.Web.Models.ChapterViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LuwAdmin.Web.Controllers
{
    [Authorize(Roles="League")]
    public class ChapterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChapterController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        // GET: Chapters
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await GetCurrentUser();
            var model = await _context.Chapters.ToListAsync();
            ViewBag.ChapterCount = model.Count();

            var viewModel = new List<IndexViewModel>();
            foreach (var c in model)
            {
                viewModel.Add(new IndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    MeetingInfo = BuildMeetingInfo(c.MeetingWeek, c.MeetingDay, c.StartTime, c.EndTime),
                    City = c.City
                });
            }
            return View(viewModel);
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            var chapterMembers = _context.MemberChapters
                .Where(m => m.ChapterId == model.Id && m.WhenExpires >= DateTime.Now)
                .Include(m => m.ApplicationUser);
            var memberInfo = new List<ChapterMemberViewModel>();
            foreach (var member in chapterMembers)
            {
                if (member.ApplicationUser != null)
                {
                    memberInfo.Add(new ChapterMemberViewModel
                    {
                        Name = member.ApplicationUser.ContactName == "Legal Name" ? member.ApplicationUser.FirstName : member.ApplicationUser.Pseudonym,
                        WhenJoined = member.WhenJoined,
                        Email = member.ApplicationUser.Email
                    });
                } 
            }

            IList<MeetingDetailsViewModel> meetingDetails = new List<MeetingDetailsViewModel>();
            var meetings = await _context.ChapterMeetings.Where(m => m.ChapterId == id).ToListAsync();
            foreach(var meeting in meetings)
            {
                var details = new MeetingDetailsViewModel
                {
                    Id = meeting.Id,
                    MeetingType = meeting.MeetingType,
                    Description = meeting.Description,
                    Venue = meeting.Venue,
                    Street1 = meeting.Street1,
                    Street2 = meeting.Street2,
                    City = meeting.City,
                    State = meeting.State,
                    Zip = meeting.Zip,
                    MeetingInfo = BuildMeetingInfo(meeting.MeetingWeek, meeting.MeetingDay, meeting.StartTime, meeting.EndTime)
                };
                meetingDetails.Add(details);
            };

            var viewModel = new ChapterDetailsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                SubName = model.SubName,
                Description = model.Description,
                Meetings = meetingDetails,
                Url = model.Url,
                Email = model.Email,
                Phone = model.Phone,
                Notes = model.Notes,
                Members = memberInfo.OrderBy(m => m.Name).ToList()
            };

            ViewBag.MemberCount = viewModel.Members.Count;
            ViewBag.Last30Days = viewModel.Members.Count(m => m.WhenJoined >= DateTime.Now.AddDays(-30));

            return View(viewModel);
        }

        // GET: Chapters/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.User = await GetCurrentUser();
            var viewModel = new ChapterCreateViewModel
            {
                State = "UT"
            };
            return View(viewModel);
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Description,Email,EndTime,MeetingDay,MeetingWeek,Name,Notes,Phone,StartTime,State,Street1,Street2,SubName,Url,Venue,Zip")] ChapterCreateViewModel viewModel)
        {
            ViewBag.User = await GetCurrentUser();

            if (ModelState.IsValid)
            {
                var chapter = new Chapter
                {
                    Name = viewModel.Name,
                    SubName = viewModel.SubName,
                    Description = viewModel.Description,
                    Url = viewModel.Url,
                    Email = viewModel.Email,
                    Phone = viewModel.Phone,
                    Notes = viewModel.Notes
                };

                _context.Add(chapter);
                await _context.SaveChangesAsync();

                var chapterMeeting = new ChapterMeeting
                {
                    ChapterId = chapter.Id,
                    Venue = viewModel.Venue,
                    Street1 = viewModel.Street1,
                    Street2 = viewModel.Street2,
                    City = viewModel.City,
                    State = viewModel.State,
                    Zip = viewModel.Zip,
                    MeetingWeek = viewModel.MeetingWeek,
                    MeetingDay = viewModel.MeetingDay,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime
                };
                _context.Add(chapterMeeting);
                await _context.SaveChangesAsync();

                Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully saved");
                return RedirectToAction("Details", new {id = chapter.Id});
            }
            return View(viewModel);
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Description,Email,EndTime,MeetingDay,MeetingWeek,Name,Notes,Phone,StartTime,State,Street1,Street2,SubName,Url,Venue,Zip")] Chapter chapter)
        {
            ViewBag.User = await GetCurrentUser();
            if (id != chapter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                    Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully saved");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new {id = chapter.Id});
            }
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = await GetCurrentUser();
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.User = await GetCurrentUser();
            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully deleted");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddMeeting(int id)
        {
            ViewBag.User = await GetCurrentUser();
            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.Chapter = chapter?.Name;

            var viewModel = new ChapterAddMeetingViewModel
            {
                ChapterId = id,
                State = "UT"
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMeeting([Bind("ChapterId,Venue,Street1,Street2,City,State,Zip,MeetingWeek,MeetingDay,StartTime,EndTime,MeetingType,Description")] ChapterAddMeetingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var meeting = new ChapterMeeting
                {
                    ChapterId = viewModel.ChapterId,
                    Description = viewModel.Description,
                    Venue = viewModel.Venue,
                    Street1 = viewModel.Street1,
                    Street2 = viewModel.Street2,
                    City = viewModel.City,
                    State = viewModel.State,
                    Zip = viewModel.Zip,
                    MeetingWeek = viewModel.MeetingWeek,
                    MeetingDay = viewModel.MeetingDay,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    MeetingType = viewModel.MeetingType
                };
                try
                {
                    _context.Add(meeting);
                    await _context.SaveChangesAsync();
                    // Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully saved");
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!ChapterExists(chapter.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction("Details", new {id = meeting.ChapterId});
            }
            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == viewModel.ChapterId);
            ViewBag.Chapter = chapter.Name;
            ViewBag.User = await GetCurrentUser();
            return View(viewModel);
        }

        public async Task<IActionResult> EditMeeting(int id)
        {
            ViewBag.User = await GetCurrentUser();
            var meeting = await _context.ChapterMeetings.SingleOrDefaultAsync(m => m.Id == id);

            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == meeting.ChapterId);
            ViewBag.Chapter = chapter?.Name;

            var viewModel = new ChapterEditMeetingViewModel
            {
                Id = meeting.Id,
                ChapterId = meeting.ChapterId,
                Description = meeting.Description,
                Venue = meeting.Venue,
                Street1 = meeting.Street1,
                Street2 = meeting.Street2,
                City = meeting.City,
                State = meeting.State,
                Zip = meeting.Zip,
                MeetingWeek = meeting.MeetingWeek,
                MeetingDay = meeting.MeetingDay,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                MeetingType = meeting.MeetingType
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMeeting([Bind("Id,ChapterId,Venue,Street1,Street2,City,State,Zip,MeetingWeek,MeetingDay,StartTime,EndTime,MeetingType,Description")] ChapterEditMeetingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var meeting = await _context.ChapterMeetings.SingleOrDefaultAsync(m => m.Id == viewModel.Id);
                meeting.Id = viewModel.Id;
                meeting.ChapterId = viewModel.ChapterId;
                meeting.Description = viewModel.Description;
                meeting.Venue = viewModel.Venue;
                meeting.Street1 = viewModel.Street1;
                meeting.Street2 = viewModel.Street2;
                meeting.City = viewModel.City;
                meeting.State = viewModel.State;
                meeting.Zip = viewModel.Zip;
                meeting.MeetingWeek = viewModel.MeetingWeek;
                meeting.MeetingDay = viewModel.MeetingDay;
                meeting.StartTime = viewModel.StartTime;
                meeting.EndTime = viewModel.EndTime;
                meeting.MeetingType = viewModel.MeetingType;

                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                    // Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully saved");
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!ChapterExists(chapter.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction("Details", new {id = meeting.ChapterId});
            }
            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == viewModel.ChapterId);
            ViewBag.Chapter = chapter.Name;
            ViewBag.User = await GetCurrentUser();
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteMeeting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.ChapterMeetings.FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }
            var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == meeting.ChapterId);
            if (chapter == null)
            {
                return NotFound();
            }
            var viewModel = new ChapterDeleteMeetingViewModel
            {
                Id = meeting.Id,
                MeetingType = meeting.MeetingType,
                Description = meeting.Description,
                Venue = meeting.Venue,
                Street1 = meeting.Street1,
                Street2 = meeting.Street2,
                City = meeting.City,
                State = meeting.State,
                Zip = meeting.Zip,
                MeetingInfo = BuildMeetingInfo(meeting.MeetingWeek, meeting.MeetingDay, meeting.StartTime, meeting.EndTime),
                ChapterName = chapter.Name
            };
            ViewBag.User = await GetCurrentUser();
            return View(viewModel);
        }

        [HttpPost, ActionName("DeleteMeeting")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMeetingConfirmed(int id)
        {
            var meeting = await _context.ChapterMeetings.SingleOrDefaultAsync(m => m.Id == id);
            _context.ChapterMeetings.Remove(meeting);
            await _context.SaveChangesAsync();
            //Response.Cookies.Append("FlashSuccess", "Chapter " + chapter.Name + " was successfully deleted");
            return RedirectToAction("Details", new {id = meeting.ChapterId});
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }

        private string BuildMeetingInfo(int? weekOfMonth, int? dayOfWeek, string startTime, string endTime)
        {
            string retVal = "";

            string week = ConvertToWeekNumber(weekOfMonth);
            string day = ConvertToDayOfWeek(dayOfWeek);
            string time = ConvertMeetingTime(startTime, endTime);

            retVal = week + day + ConvertMeetingTime(startTime, endTime);

            return retVal;
        }

        private string ConvertMeetingTime(string startTime, string endTime)
        {
            var retVal = startTime;
            
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                retVal += "-";
            }
            retVal += endTime;
            return retVal;
        }
        
        private string ConvertToWeekNumber(int? weekOfMonth)
        {
            switch (weekOfMonth)
            {
                case 1:
                    return "First ";
                case 2:
                    return "Second ";
                case 3:
                    return "Third ";
                case 4:
                    return "Fourth ";
                default:
                    return "";
            }
        }

        private string ConvertToDayOfWeek(int? dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return "Sunday ";
                case 2:
                    return "Monday ";
                case 3:
                    return "Tuesday ";
                case 4:
                    return "Wednesday ";
                case 5:
                    return "Thursday ";
                case 6:
                    return "Friday ";
                case 7:
                    return "Saturday ";
                default:
                    return "";
            }
        }
    }
}
