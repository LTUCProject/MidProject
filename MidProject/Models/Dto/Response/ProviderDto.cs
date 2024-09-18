using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Response
{
    public class ProviderDto
    {
        public int ProviderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }

}
