using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }

        [Required]
        public string AccountId { get; set; } // Foreign key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }


        public Account Account { get; set; } // Navigation property
        public ICollection<ServiceInfo> Services { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; } // New collection for ownership


        public ICollection<Session> session { get; set; }



    }
}
