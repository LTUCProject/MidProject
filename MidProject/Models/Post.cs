namespace MidProject.Models
{
    public class Post
    {
        public int PostId { get; set; }  
        public int UserId { get; set; }  
        public User User { get; set; } 
        public string Title { get; set; } 
        public string Content { get; set; } 
        public DateTime Date { get; set; }  
        public ICollection<Comment> Comments { get; set; }


    }
}
