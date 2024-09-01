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
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public ICollection<Booking> Bookings { get; set; }
    }
}
