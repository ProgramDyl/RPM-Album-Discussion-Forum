using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly RPMForumContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CommentsController(RPMForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var RPMForumContext = _context.Comment.Include(c => c.Discussion);
            


            return View(await RPMForumContext.ToListAsync());
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get id of logged in user
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .Include("Comments")
                .Where(m => m.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            //set discussionID for comment's fk
            ViewData["DiscussionId"] = id;

            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,DiscussionId")] Comment comment)
        {
            comment.CreateDate = DateTime.Now;

            //get the photos and ensure looged in user is owner

            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect back to the Discussions Edit page
                return RedirectToAction("DiscussionDetails", "Home", new { id = comment.DiscussionId });
            }

            ViewData["DiscussionId"] = comment.DiscussionId;
            return View(comment);
        }


        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            //get the photos and ensure looged in user is owner


            if (id == null)
            {
                return NotFound();
            }

            

            var comment = await _context.Comment
                .Include(c => c.Discussion)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
