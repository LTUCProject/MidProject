using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class PostDto
    {
        public int ClientId { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
