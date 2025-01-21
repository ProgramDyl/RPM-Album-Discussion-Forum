using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    public class HomeController : Controller
    {
       
        //constructor
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {

            List<Post> post = new List<Post>();

            Post post1 = new Post();
            post1.PostId = 1;
            post1.Title = "Loving this Miles Davis album";
            post1.Body = "Found this \"Miles Davis: Compilation\" yesterday, It's incredible!";
            post1.CreatedAt = DateTime.Now;

            Post post2 = new Post();
            post2.PostId = 2;
            post2.Title = "Kenny G sucks!!!!1";
            post2.Body = "hate him that is all";
            post2.CreatedAt = DateTime.Now;

            Post post3 = new Post();
            post3.PostId = 3;
            post3.Title = "has anyone ever heard of tupac?? this guy is pretty good";
            post3.Body = "like the title says";
            post3.CreatedAt = DateTime.Now;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
