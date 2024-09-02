namespace MidProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
