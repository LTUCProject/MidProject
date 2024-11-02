using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MidProject.Models
{
    public class ChargingStation
    {

        public int ChargingStationId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } //

        public double Latitude { get; set; } //
        public double Longitude { get; set; } //

        public string StationLocation { get; set; } //

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } //

        public bool HasParking { get; set; } //
        public string Status { get; set; } //
        public string PaymentMethod { get; set; } //

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
