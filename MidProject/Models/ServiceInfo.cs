using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class ServiceInfo
    {
        public int ServiceInfoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }

        [Required]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } 

    }
}
