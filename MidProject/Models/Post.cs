namespace MidProject.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
