namespace MidProject.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public int ChargingStationId { get; set; } // Foreign key
        public ChargingStation ChargingStation { get; set; } //Navigator
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnergyConsumed { get; set; }
        public int Cost { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
