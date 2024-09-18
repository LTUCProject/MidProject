using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request2
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; } // Foreign key to Client
        public int ChargingStationId { get; set; } // Foreign key to ChargingStation
        public int VehicleId { get; set; } // Foreign key to Vehicle
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending"; // Default to "Pending"

        public int Cost { get; set; } = 0; // Default to 0
        
    }
}
