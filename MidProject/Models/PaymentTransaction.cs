namespace MidProject.Models
{
    public class PaymentTransaction
    {
        public int PaymentTransactionId { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }



    }
}
