using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;
using System.Threading.Tasks;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicForumContext _context;

        public HomeController(MusicForumContext context)
        {
            _context = context;
        }

        // home - show all discussion threads
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
