using System.Runtime.CompilerServices;

namespace MusicForum.Models
{

    public class Comment
    {
        public int CommentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        //fk
        public int DiscussionId {  get; set; }

        //nav prop
        public Discussion? Discussion { get; set; }


        
    }

    
}
