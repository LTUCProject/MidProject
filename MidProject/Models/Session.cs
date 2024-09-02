namespace MidProject.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnergyConsumed { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
