using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;

namespace Notes.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeetingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Meetings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Meeting.Include(m => m.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .Include(m => m.Client)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }
            ViewData["Users"] = this.GetUserOfMeetings(id);
            return View(meeting);
        }

        // GET: Meetings/Create
        public async Task<IActionResult> Create()
        {

            ViewData["Users"] = new SelectList(_context.Users.ToList<ApplicationUser>(), "Id", "UserName");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeetingViewModel meeting)
        {
            if (ModelState.IsValid)
            {
                var users = Request.Form.ToList();
                var meetingModel = new Meeting()
                {
                    Title = meeting.Title,
                    DateAndTime = meeting.DateAndTime,
                    IsVirtual = meeting.IsVirtual,
                    ClientId = meeting.ClientId
                };
                var result = _context.Add(meetingModel);
                await _context.SaveChangesAsync();

                foreach (var user in users[1].Value.ToList())
                {
                    var me = new MeetingsForUser()
                    {
                        UserId = user,
                        MeetingId = meetingModel.Id
                    };
                    _context.MeetingsForUser.Add(me);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", meeting.ClientId);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting.SingleOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }
            var meet = new MeetingViewModel();
            meet.Id = meeting.Id;
            meet.IsVirtual = meeting.IsVirtual;
            meet.DateAndTime = meeting.DateAndTime;
            meet.Title = meeting.Title;
            meet.ClientId = meeting.ClientId;

            ViewData["Users"] = new SelectList(_context.Users.ToList<ApplicationUser>(), "Id", "UserName");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", meeting.ClientId);
            return View(meet);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, MeetingViewModel meeting)
        {
            if (id != meeting.Id)
            {
                return NotFound();
            }
            var meetingModel = new Meeting();

            if (ModelState.IsValid)
            {
                try
                {
                    meetingModel = new Meeting()
                    {
                        Id = id,
                        Title = meeting.Title,
                        DateAndTime = meeting.DateAndTime,
                        IsVirtual = meeting.IsVirtual,
                        ClientId = meeting.ClientId
                    };
                    var result = _context.Update(meetingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var users = Request.Form.ToList();
                foreach (var user in users[2].Value.ToList())
                {
                    var me = new MeetingsForUser()
                    {
                        UserId = user,
                        MeetingId = meetingModel.Id
                    };
                    var meetingsForUser = (from mu in _context.MeetingsForUser
                                           where mu.MeetingId == id && mu.UserId == user
                                           select new MeetingsForUser
                                           {
                                               Id = mu.Id,
                                               MeetingId = mu.MeetingId,
                                               UserId = mu.UserId
                                           }).ToList();
                    if (meetingsForUser.Count == 0)
                    {
                        _context.MeetingsForUser.Add(me);
                        _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", meeting.ClientId);
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var meeting = await _context.Meeting
                .Include(m => m.Client)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _context.Database.ExecuteSqlCommand("Delete from MeetingsForUser where MeetingId = {0}", id);
            var meeting = await _context.Meeting.SingleOrDefaultAsync(m => m.Id == id);
            _context.Meeting.Remove(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IEnumerable<ApplicationUser> GetUserOfMeetings(string id)
        {
            var meetingsForUser =  (from mu in _context.MeetingsForUser
                                    where mu.MeetingId == id
                                    select new MeetingsForUser
                                    {
                                        Id = mu.Id,
                                        MeetingId = mu.MeetingId,
                                        UserId = mu.UserId
                                    }).ToList();
            MeetingViewModel U = new MeetingViewModel(); 
            foreach (var item in meetingsForUser)
            {
               U.ListUsers.Add((_context.Users.Find(item.UserId)));
            }
            IEnumerable<ApplicationUser> users = U.ListUsers;
            return users;
        }

        private bool MeetingExists(string id)
        {
            return _context.Meeting.Any(e => e.Id == id);
        }
    }
}
