using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class MaintenanceLog
    {
        public int MaintenanceLogId { get; set; }

        [Required]
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string PerformedBy { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Details { get; set; }
        public int Cost { get; set; }
    }
}
