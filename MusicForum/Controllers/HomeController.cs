using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MusicForum.Models;
using MusicForum.Data;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicForumContext _context;

        // Constructor
        public HomeController(MusicForumContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch discussions from the database
            List<Discussion> discussions = _context.Discussion.ToList();

            // Pass the discussions to the view
            return View(discussions);
        }

        // Discussion page
        public IActionResult Discussions(int id)
        {
            // Fetch a specific discussion by id
            Discussion discussion = _context.Discussion.Find(id);
            return View(discussion);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
