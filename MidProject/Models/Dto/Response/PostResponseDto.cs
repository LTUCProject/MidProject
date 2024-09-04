namespace MidProject.Models.Dto.Response
{
    public class PostResponseDto
    {
        public int PostId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<CommentResponseDto> Comments { get; set; }
    }
}
