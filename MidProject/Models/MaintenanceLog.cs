namespace MidProject.Models
{
    public class MaintenanceLog
    {
        public int MaintenanceLogId { get; set; }
        public int ChargingStationId { get; set; } // Foreign key
        public ChargingStation ChargingStation { get; set; } //Navigator
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
