namespace MusicForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }
        
        public string Title { get; set; } = string.Empty;

        public string Content {  get; set; } = string.Empty;

        public string ImageFileName { get; set; } = string.Empty 
        
        public DateTime CreateDate { get; set; }
        
        //nav prop
        public Comment? Comment { get; set; }
    }
}
