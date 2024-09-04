using MidProject.Models;

namespace MidProject.Models.Dto.Request
{
    public class PaymentTransactionDto
    {
        public int PaymentTransactionId { get; set; }
        public int SessionId { get; set; } // Foreign key
        public int ClientId { get; set; } // Foreign key
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}