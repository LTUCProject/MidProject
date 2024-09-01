namespace MidProject.Models
{
    public class PaymentTransaction
    {
        public int PaymentTransactionId { get; set; }
        public int SessionId { get; set; } // Foreign key
        public Session Session { get; set; } //Navigator
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
