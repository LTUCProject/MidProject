namespace MidProject.Models.Dto.Request
{
    public class MaintenanceLogDto
    {
        public int MaintenanceLogId { get; set; }
        public int ChargingStationId { get; set; } // Foreign key
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
