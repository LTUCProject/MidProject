namespace MidProject.Models.Dto.Response
{
    public class CommentResponseDto
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
