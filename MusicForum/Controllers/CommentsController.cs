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
        private readonly RPMForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(RPMForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Comments
        public async Task<IActionResult> Index()
        {
            //filter based on logged in user 
            var userId = _userManager.GetUserId(User);

            var comments = await _context.Comment
                .Include(c => c.Discussion)
                .Where(c => c.Discussion.ApplicationUserId == userId) //thread is owned by user 
                .ToListAsync();


            return View(comments);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Check if the discussion exists (any owner) 
            var discussionExists = await _context.Discussion.AnyAsync(d => d.DiscussionId == id);
            if (!discussionExists)
            {
                return NotFound();
            }

            // Set discussionID for comment's foreign key
            ViewData["DiscussionId"] = id;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,DiscussionId")] Comment comment)
        {
            comment.CreateDate = DateTime.Now;

            // Set the owner of the comment
            var userId = _userManager.GetUserId(User);
            comment.ApplicationUserId = userId;

            // Validate that the DiscussionId exists
            var discussionExists = await _context.Discussion.AnyAsync(d => d.DiscussionId == comment.DiscussionId);
            if (!discussionExists)
            {
                ModelState.AddModelError("DiscussionId", "The specified discussion does not exist.");
                ViewData["DiscussionId"] = comment.DiscussionId;
                return View(comment);
            }

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

            //get the photos and ensure logged in user is owner

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

        //private bool CommentExists(int id)
        //{
        //    return _context.Comment.Any(e => e.CommentId == id);
        //}
    }
}
