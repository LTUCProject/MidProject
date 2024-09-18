using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class BookingUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string NewStatus { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Cost must be a non-negative integer.")]
        public int NewCost { get; set; }
    }
}
