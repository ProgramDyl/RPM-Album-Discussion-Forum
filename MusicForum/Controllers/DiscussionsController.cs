using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicForum.Data;
using MusicForum.Models;

namespace MusicForum.Controllers
{
    [Authorize] 
    public class DiscussionsController : Controller
    {
        private readonly RPMForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiscussionsController(RPMForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Discussions by user id
        public async Task<IActionResult> Index()
        {
            //get threads of person logged on 
            var userId = _userManager.GetUserId(User);

            var discussions = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId)
                .ToListAsync();

            return View(discussions);
        }

     

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile,CreateDate")] Discussion discussion)
        {

            //init datetime prop
            discussion.CreateDate = DateTime.Now;

            //rename uploaded file to a guid (unique filename). Set before saved in db.
            discussion.ImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile?.FileName);

            //sets owner of the record
            var userId = _userManager.GetUserId(User);
            //set current owner's ID to the discussion
            discussion.ApplicationUserId = userId;


            if (ModelState.IsValid)
            {

                _context.Add(discussion);
                await _context.SaveChangesAsync();

                // Save the uploaded file after the photo is saved in the database.
                if (discussion.ImageFile != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", discussion.ImageFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }
                }

                
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get id of logged in user
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .Where(m => m.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFile,ImageFileName,CreateDate,ApplicationUserId")] Discussion discussion)
        {
            
            

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing discussion from the database
                    var existingDiscussion = await _context.Discussion
                        .AsNoTracking()
                        .FirstOrDefaultAsync(d => d.DiscussionId == id);

                    if (existingDiscussion == null)
                    {
                        return NotFound();
                    }

                    
                    // Save the uploaded file before updating the discussion in the database.
                    if (discussion.ImageFile != null)
                    {
                        // Generate a unique filename for the new image
                        discussion.ImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);

                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", discussion.ImageFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await discussion.ImageFile.CopyToAsync(fileStream);
                        }
                        //Delete the old file if it exists
                        if (!string.IsNullOrEmpty(existingDiscussion.ImageFileName))
                        {
                            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", existingDiscussion.ImageFileName);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                    }
                    else
                    {
                        // Retain the existing file name if no new file is uploaded
                        discussion.ImageFileName = existingDiscussion.ImageFileName;
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }


        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get id of logged in user
            var userId = _userManager.GetUserId(User);

            var discussion = await _context.Discussion
                .Where(m => m.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //get id of logged in user
            var userId = _userManager.GetUserId(User);

            //grab thread WITH comments
            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .Where(m => m.ApplicationUserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);


            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index)); 

        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
