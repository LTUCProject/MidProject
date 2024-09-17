namespace MidProject.Models.Dto.Response
{
    public class PostResponseDto
    {
        public int PostId { get; set; }
        public string AccountId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<CommentResponseDto> Comments { get; set; }
    }
}
