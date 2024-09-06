using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class ServiceInfoRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Contact { get; set; }

        public string Type { get; set; }

        [Required]
        public int ProviderId { get; set; }
    }
}
