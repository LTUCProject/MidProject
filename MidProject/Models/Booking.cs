namespace MidProject.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public int ServiceInfoId { get; set; } // Foreign key
        public ServiceInfo ServiceInfo { get; set; } //Navigator
        public int VehicleId { get; set; } // Foreign key
        public Vehicle Vehicle { get; set; } //Navigator
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }


    }
}
