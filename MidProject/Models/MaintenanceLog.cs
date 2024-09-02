namespace MidProject.Models
{
    public class MaintenanceLog
    {
        public int MaintenanceLogId { get; set; }
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
