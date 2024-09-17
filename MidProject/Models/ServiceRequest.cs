using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required]
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }

        [Required]
        public int ClientId { get; set; } // Matches IdentityUser key type
        public Client Client { get; set; }

        [Required]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        [Required]
        public int VehicleId { get; set; }  // New field for the associated vehicle
        public Vehicle Vehicle { get; set; } // Navigation property for Vehicle

        public string Status { get; set; }
    }
}
