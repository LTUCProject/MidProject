using System.ComponentModel.DataAnnotations;
namespace MidProject.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }

        [Required]
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Range(0, int.MaxValue)]
        public int Cost { get; set; }

    }
}
