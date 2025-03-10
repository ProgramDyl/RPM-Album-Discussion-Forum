using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;
using System.Threading.Tasks;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly RPMForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(RPMForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // home page - show all discussion threads
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                .Include(d => d.Comments)
                .Include(d => d.ApplicationUser)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

            return View(discussions);
        }

        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var discussions = await _context.Discussion
                .Include(d => d.Comments)
                .Include(d => d.ApplicationUser)
                .Where(d => d.ApplicationUserId == id)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

            ViewBag.User = user; // Pass the user to the view
            return View(discussions);
        }


        // discussion details - displays the details of a discussion
        public async Task<IActionResult> DiscussionDetails(int id)
        {
            var discussion = await _context.Discussion
                .Include(m => m.ApplicationUser)
                .Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            return View(discussion);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
