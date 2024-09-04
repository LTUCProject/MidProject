using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class PaymentTransaction
    {
        public int PaymentTransactionId { get; set; }

        [Required]
        public int SessionId { get; set; }
        public Session Session { get; set; }

        [Required]

        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }

        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }



    }
}
