using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [Range(1900, int.MaxValue)]
        public int Year { get; set; }
        public int BatteryCapacity { get; set; }
        public string ElectricType { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
