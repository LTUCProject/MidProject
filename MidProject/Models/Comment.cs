namespace MidProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }  
        public int UserId { get; set; }  // Foreign key to User
        public User User { get; set; }  //Navigator
        public int PostId { get; set; }  // Foreign key to Post
        public Post Post { get; set; }  //Navigator
        public string Content { get; set; }  
        public DateTime Date { get; set; }  
    }
}
