namespace MidProject.Models.Dto.Response
{
    public class MaintenanceLogDtoResponse
    {
        public int MaintenanceLogId { get; set; }
        public int ChargingStationId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
