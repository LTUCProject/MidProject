using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class CommentDto
    {
        public int ClientId { get; set; } 
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
