namespace MidProject.Models.Dto.Request
{
    public class MaintenanceLogDto
    {
        public int ChargingStationId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
