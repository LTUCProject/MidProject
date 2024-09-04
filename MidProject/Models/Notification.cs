using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [Required]
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
