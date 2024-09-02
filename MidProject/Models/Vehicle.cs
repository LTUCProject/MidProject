namespace MidProject.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int BatteryCapacity { get; set; }
        public string ElectricType { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
