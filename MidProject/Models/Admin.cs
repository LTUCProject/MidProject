using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required]
        public string AccountId { get; set; } // Foreign key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Email { get; set; }

        public Account Account { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; }
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; }

    }
}
