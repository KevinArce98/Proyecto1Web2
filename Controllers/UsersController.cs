using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Notes.Models.AccountViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Controllers
{

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = (from ur in _context.UserRoles
                              join r in _context.Roles on ur.RoleId equals r.Id
                              join u in _context.Users on ur.UserId equals u.Id
                              select new UserRoleInfo
                              {
                                 Id = u.Id,
                                 Username = u.UserName,
                                 Role = r.Name
                              }).ToList();

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            var allRoles = (_context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
             new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList());
            ViewBag.Roles = allRoles;
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = (from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join u in _context.Users on ur.UserId equals u.Id
                        where u.Id == id
                        select new RegisterViewModel
                        {
                            Id = u.Id,
                            Username = u.UserName,
                            Role = r.Name
                        }).ToList();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = (from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join u in _context.Users on ur.UserId equals u.Id
                        where u.Id == id
                        select new RegisterViewModel
                        {
                            Id = u.Id,
                            Username = u.UserName,
                            Role = r.Name
                        }).ToList();
            if (user == null)
            {
                return NotFound();
            }
            var allRoles = (_context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
             new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList());
            ViewBag.Roles = allRoles;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RegisterViewModel User)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = User.Username;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.RemoveFromRoleAsync(user, User.Role);
                await _userManager.AddToRoleAsync(user, User.RoleId);

                return RedirectToAction(nameof(Index));
            }
            
            var list = new List<RegisterViewModel>();
            list.Add(User);
            return View(list);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
