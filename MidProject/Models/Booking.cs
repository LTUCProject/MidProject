namespace MidProject.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }

    }
}
