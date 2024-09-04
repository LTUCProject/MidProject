namespace MidProject.Models
{
    public class ChargingStation
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public int ProviderId { get; set; } // New foreign key
        public Provider Provider { get; set; } // Navigation property
        public ICollection<Charger> Chargers { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
