namespace MidProject.Models.Dto.Request
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }  // Foreign key to User
        public int PostId { get; set; }  // Foreign key to Post
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
