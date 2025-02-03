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
        [Display(Name = "Photo")]
        public IFormFile? ImageFile { get; set; } // nullable!

        public string ImageFileName { get; set; } = string.Empty; 
        
        public DateTime CreateDate { get; set; }

        //nav prop
        public List<Comment>? Comments { get; set; } 
    }
}
