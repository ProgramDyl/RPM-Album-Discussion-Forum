// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MusicForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            ///////////////////////////////////////
            // BEGIN: ApplicationUser Custom Fields
            ///////////////////////////////////////

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nickname")]
            public string Nickname { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Favourite Album")]
            public string FavouriteAlbum { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Location")]
            public string Location { get; set; }

            public string ImageFilename { get; set; }

            [Display(Name = "Change Profile Picture")]
            public IFormFile ImageFile { get; set; }
            ///////////////////////////////////////
            // END: ApplicationUser Custom Fields
            ///////////////////////////////////////


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code.This API may change or be removed in future releases.
            /// </summary>

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            

            Username = userName;

            Input = new InputModel
            {

                /////////////////////////////////////////
                /// BEGIN: ApplicationUser Custom Fields
                /////////////////////////////////////////
                
                Nickname = user.Name,
                FavouriteAlbum = user.FavouriteAlbum,
                Location = user.Location,
                ImageFilename = user.ImageFilename
                /////////////////////////////////////////
                /// END: ApplicationUser Custom Fields
                /////////////////////////////////////////
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            /////////////////////////////////////////
            /// BEGIN: ApplicationUser Custom Fields
            ///////////////////////////////////////// 

            if (Input.Nickname != user.Name)
            {
                user.Name = Input.Nickname;
            }

            if (Input.FavouriteAlbum != user.FavouriteAlbum)
            {
                user.FavouriteAlbum = Input.FavouriteAlbum;
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }

            //Update the profile picture
            if (Input.ImageFile != null)
            {
                string imageFilename = Guid.NewGuid().ToString() + Path.GetExtension(Input.ImageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_img", imageFilename);

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(filestream);
                }
                user.ImageFilename = imageFilename;
            }

            await _userManager.UpdateAsync(user);

            /////////////////////////////////////////
            /// END: ApplicationUser Custom Fields
            ///////////////////////////////////////// 
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
