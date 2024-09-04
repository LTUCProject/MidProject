using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required]
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }

        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
