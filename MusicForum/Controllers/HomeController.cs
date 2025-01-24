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

            List<Discussion> discussions = new List<Discussion>();

            Discussion discussion1 = new Discussion();
            discussion1.DiscussionId = 1;
            discussion1.Title = "Test";
            discussion1.Content = "Testing 123";
            discussion1.ImageFileName = "testing";
            discussion1.CreateDate = DateTime.Now;

            Discussion discussion2 = new Discussion();
            discussion2.DiscussionId = 2;
            discussion2.Title = "Test";
            discussion2.Content = "testing";


            discussions.Add(discussion1);
            discussions.Add(discussion2);

            



            return View(discussions);
        }

        public IActionResult Discussions(int id)
        {
            Discussion discussion = new Discussion();
            discussion.DiscussionId = id;

            discussion.Title = "Test";
            discussion.Content = "testing";
            discussion.ImageFileName = "testing";
            discussion.CreateDate = DateTime.Now;
            return View(discussion);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
