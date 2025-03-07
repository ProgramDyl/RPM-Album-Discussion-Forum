using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }
        
        public string Title { get; set; } = string.Empty;

        public string Content {  get; set; } = string.Empty;

        // Property for file upload, not mapped in EF
        [NotMapped]
        [Display(Name = "Album Art")]
        public IFormFile? ImageFile { get; set; } // nullable!

        [Display(Name = "Image")]
        public string ImageFileName { get; set; } = string.Empty;

        [Display(Name = "Date Posted")]
        public DateTime CreateDate { get; set; }

        //nav prop
        [Display(Name = "Comments")] 
        public List<Comment>? Comments { get; set; } 


        //user properties
        //public string ApplicationUserId { get; set; } = string.Empty;

        ////nav prop
        //public ApplicationUser? ApplicationUser { get; set; }


    }
}
