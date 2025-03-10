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

        public HomeController(RPMForumContext context)
        {
            _context = context;
        }

        // home page - show all discussion threads
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                .Include(d => d.Comments) 
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();

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
