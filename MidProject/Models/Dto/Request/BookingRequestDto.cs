using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class BookingRequestDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ServiceInfoId { get; set; }

        [Required]
        public int VehicleId { get; set; }

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
