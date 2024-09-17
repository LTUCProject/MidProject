using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required]
        public string AccountId { get; set; } // Changed to string to match IdentityUser key type
        public Account Account { get; set; }

        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
