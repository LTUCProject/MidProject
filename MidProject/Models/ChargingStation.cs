using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class ChargingStation
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }

        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }

        [Required]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public ICollection<Charger> Chargers { get; set; }
        public ICollection<Booking> Bookings { get; set; } 
        public ICollection<Session> Sessions { get; set; }
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; }
        public ICollection<ChargingStationFavorite> ChargingStationFavorites { get; set; }
    }
}
